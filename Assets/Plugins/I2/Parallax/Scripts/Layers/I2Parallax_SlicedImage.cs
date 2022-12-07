using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Parallax
{
    [AddComponentMenu("I2/Parallax/I2Parallax Sliced Image")]
    [RequireComponent(typeof(Graphic))]
    public class I2Parallax_SlicedImage : MonoBehaviour,  IMeshModifier, IParallaxLayer
    {
        [Header("Depth")]
        [Range(0, 130)] public float _DepthOut;
        [Range(0, 130)] public float _DepthIn;

        [Header("Margin")]
        [Range(0,1)]public float _Slice_Left = 0.3f;
        [Range(0,1)]public float _Slice_Right = 0.3f;
        [Range(0,1)]public float _Slice_Top = 0.3f;
        [Range(0,1)]public float _Slice_Bottom = 0.3f;

        [Space(10)]
        [Range(0, 3)]public int _Subdivisions = 0;

        [Header("Constraint")]
        public bool _Constrain_Top;
        public bool _Constrain_Bottom;
        public bool _Constrain_Left;
        public bool _Constrain_Right;

        [Header("Expand")]
        [Range(0,1)]public float _AutoExpand_Top;
        [Range(0,1)]public float _AutoExpand_Bottom;
        [Range(0,1)]public float _AutoExpand_Left;
        [Range(0,1)]public float _AutoExpand_Right;

        //[Header("Inside Vertices")]
        public Vector2 _DepthFactor_TopLeft     = Vector2.one;
        public Vector2 _DepthFactor_TopRight    = Vector2.one;
        public Vector2 _DepthFactor_BottomLeft  = Vector2.one;
        public Vector2 _DepthFactor_BottomRight = Vector2.one;



        List<UIVertex> mVertices = new List<UIVertex>();
        List<int> mIndices       = new List<int>();
        Vector3 mPerspDir        = Vector3.zero;
        Graphic mGraphic;


        public void OnEnable()
        {
            mGraphic = GetComponent<Graphic>();
            I2Parallax_Manager.RegisterLayer(this);
        }

        public void OnDisable()
        {
            I2Parallax_Manager.UnregisterLayer(this);
        }

        public void UpdateLayer(Vector2 dir)
        {
            var v = I2Parallax_Manager.singleton.GetViewPosition();
            mPerspDir = (Vector2)v;
            mGraphic.SetVerticesDirty();
        }

        private void OnValidate()
        {
            if (!mGraphic)
                mGraphic = GetComponent<Graphic>();
            mGraphic.SetVerticesDirty();
        }

        public void ModifyMesh(Mesh mesh)
        {
            if (!enabled) return;

            //if (!this.IsActive()) return;

            using (VertexHelper vertexHelper = new VertexHelper(mesh))
            {
                this.ModifyMesh(vertexHelper);
                vertexHelper.FillMesh(mesh);
            }
        }

        public void ModifyMesh(VertexHelper vh)
        {
            // 1----------2 
            // |\      /  |
            // |  5---6   |
            // |  |   |   |
            // |  4---7   |
            // | /      \ |
            // 0----------3
            if (vh.currentVertCount != 4)
            {
                Debug.LogErrorFormat("Image {0} has to be a non-sliced image", name);
            }

            float scale = Screen.dpi / 100.0f;
            float speedIn = _DepthIn * scale;
            float speedOut = _DepthOut * scale;

            mVertices.Clear();
            vh.PopulateUIVertex(ref tempVert, 0); mVertices.Add(tempVert);
            vh.PopulateUIVertex(ref tempVert, 1); mVertices.Add(tempVert);
            vh.PopulateUIVertex(ref tempVert, 2); mVertices.Add(tempVert);
            vh.PopulateUIVertex(ref tempVert, 3); mVertices.Add(tempVert);

            var vSpeedIn_LeftBottom   = new Vector2(_Slice_Left > 0 ? speedIn : speedOut,     _Slice_Bottom > 0 ? speedIn : speedOut);
            var vSpeedIn_RightTop     = new Vector2(_Slice_Right > 0 ? speedIn : speedOut,    _Slice_Top > 0 ? speedIn : speedOut);
            var vSpeedIn_LeftTop      = new Vector2(vSpeedIn_LeftBottom.x, vSpeedIn_RightTop.y);
            var vSpeedIn_RightBottom  = new Vector2(vSpeedIn_RightTop.x, vSpeedIn_LeftBottom.y);

            AddVertex( new Vector2(_Slice_Left, _Slice_Bottom),         Vector2.Scale(vSpeedIn_LeftBottom, _DepthFactor_BottomLeft),                float.MinValue, _Constrain_Left&&_Slice_Left<=float.Epsilon?0:float.MaxValue,           float.MinValue, _Constrain_Bottom&&_Slice_Bottom<=float.Epsilon?0:float.MaxValue);
            AddVertex( new Vector2(_Slice_Left, 1-_Slice_Top),          Vector2.Scale(vSpeedIn_LeftTop, _DepthFactor_TopLeft),                      float.MinValue, _Constrain_Left&&_Slice_Left<=float.Epsilon?0:float.MaxValue,           _Constrain_Top&&_Slice_Top<=float.Epsilon?0:float.MinValue, float.MaxValue);
            AddVertex( new Vector2(1-_Slice_Right, 1-_Slice_Top),       Vector2.Scale(vSpeedIn_RightTop, _DepthFactor_TopRight),                    _Constrain_Right&&_Slice_Right<=float.Epsilon?0:float.MinValue, float.MaxValue,         _Constrain_Top&&_Slice_Top<=float.Epsilon?0:float.MinValue, float.MaxValue);
            AddVertex( new Vector2(1-_Slice_Right, _Slice_Bottom),      Vector2.Scale(vSpeedIn_RightBottom, _DepthFactor_BottomRight),              _Constrain_Right&&_Slice_Right<=float.Epsilon?0:float.MinValue, float.MaxValue,         float.MinValue, _Constrain_Bottom&&_Slice_Bottom<=float.Epsilon?0:float.MaxValue);

            ApplyVertexSpeed(0, speedOut,   -_AutoExpand_Left,      -_AutoExpand_Bottom,        float.MinValue, _Constrain_Left?0:float.MaxValue,       float.MinValue, _Constrain_Bottom?0:float.MaxValue);
            ApplyVertexSpeed(1, speedOut,   -_AutoExpand_Left,      _AutoExpand_Top,            float.MinValue, _Constrain_Left?0:float.MaxValue,       _Constrain_Top?0:float.MinValue, float.MaxValue);
            ApplyVertexSpeed(2, speedOut,   _AutoExpand_Right,      _AutoExpand_Top,            _Constrain_Right?0:float.MinValue, float.MaxValue,      _Constrain_Top?0:float.MinValue, float.MaxValue);
            ApplyVertexSpeed(3, speedOut,   _AutoExpand_Right,      -_AutoExpand_Bottom,        _Constrain_Right?0:float.MinValue, float.MaxValue,      float.MinValue, _Constrain_Bottom?0:float.MaxValue);

            
            //if (mIndices.Count!=10*3)
            {
                mIndices.Clear();
                AddQuad(4, 5, 6, 7, 0);
                if (_Slice_Left>0)      AddQuad(0, 1, 5, 4, 0);
                if (_Slice_Top>0)       AddQuad(1, 2, 6, 5, 0);
                if (_Slice_Right>0)     AddQuad(2, 3, 7, 6, 0);
                if (_Slice_Bottom > 0)  AddQuad(3, 0, 4, 7, 0);
            }

            //vertices[0].
            vh.Clear();
            vh.AddUIVertexStream(mVertices, mIndices);
        }

        UIVertex tempVert = new UIVertex();
        void ApplyVertexSpeed(int idx, float speed, float offsetX, float offsetY, float xMin, float xMax, float yMin, float yMax)
        {
            tempVert = mVertices[idx];
            Vector3 pos = mPerspDir;
            pos.x += offsetX;
            pos.y += offsetY;

            if (pos.x < xMin) pos.x = xMin;
            if (pos.y < yMin) pos.y = yMin;
            if (pos.x > xMax) pos.x = xMax;
            if (pos.y > yMax) pos.y = yMax;

            tempVert.position += speed* pos;
            mVertices[idx] = tempVert;
        }

        void AddVertex( Vector2 uv, Vector2 speed, float xMin, float xMax, float yMin, float yMax)
        {
            Vector3 v0 = mVertices[0].position;
            Vector3 v1 = mVertices[1].position;
            Vector3 v2 = mVertices[2].position;
            Vector3 v3 = mVertices[3].position;

            Vector3 delta = mPerspDir;
            if (delta.x < xMin) delta.x = xMin;
            if (delta.y < yMin) delta.y = yMin;
            if (delta.x > xMax) delta.x = xMax;
            if (delta.y > yMax) delta.y = yMax;

            Vector3 top = Vector3.Lerp(v1, v2, uv.x);
            Vector3 bottom = Vector3.Lerp(v0, v3, uv.x);
            tempVert = mVertices[0];
            tempVert.position = Vector3.Lerp(bottom, top, uv.y) + Vector3.Scale(delta, speed);

            tempVert.uv0 = uv;
            mVertices.Add(tempVert);
        }
        int AddVertex(int idx0, int idx1)
        {
            tempVert = mVertices[idx0];
            tempVert.position = Vector3.Lerp(tempVert.position, mVertices[idx1].position, 0.5f);
            tempVert.uv0 = Vector2.Lerp(tempVert.uv0, mVertices[idx1].uv0, 0.5f);
            mVertices.Add(tempVert);
            return mVertices.Count - 1;
        }
        int AddVertex(int idx0, int idx1, int idx2, int idx3, int idx4, int idx5, int idx6, int idx7)
        {
            tempVert = mVertices[idx0];
            tempVert.position = (mVertices[idx0].position + mVertices[idx1].position + mVertices[idx2].position + mVertices[idx3].position + mVertices[idx4].position + mVertices[idx5].position + mVertices[idx6].position + mVertices[idx7].position) / 8.0f;
            tempVert.uv0 = (mVertices[idx0].uv0 + mVertices[idx1].uv0 + mVertices[idx2].uv0 + mVertices[idx3].uv0 + mVertices[idx4].uv0 + mVertices[idx5].uv0 + mVertices[idx6].uv0 + mVertices[idx7].uv0) / 8.0f;
            mVertices.Add(tempVert);
            return mVertices.Count - 1;
        }


        void AddQuad( int idx0, int idx1, int idx2, int idx3, int subdivision )
        {
            if (subdivision >= _Subdivisions)
            {
                mIndices.Add(idx0); mIndices.Add(idx1); mIndices.Add(idx3);
                mIndices.Add(idx3); mIndices.Add(idx1); mIndices.Add(idx2);
                return;
            }

            // 0---4---1
            // |   |   |
            // 7---8---5
            // |   |   |
            // 3---6---2

            int idx4 = AddVertex(idx0, idx1);
            int idx5 = AddVertex(idx1, idx2);
            int idx6 = AddVertex(idx2, idx3);
            int idx7 = AddVertex(idx3, idx0);
            int idx8 = AddVertex(idx0, idx1, idx2, idx3, idx4, idx5, idx6, idx7);

            AddQuad(idx7, idx0, idx4, idx8, subdivision + 1);
            AddQuad(idx4, idx1, idx5, idx8, subdivision + 1);
            AddQuad(idx5, idx2, idx6, idx8, subdivision + 1);
            AddQuad(idx6, idx3, idx7, idx8, subdivision + 1);
        }
    }
}