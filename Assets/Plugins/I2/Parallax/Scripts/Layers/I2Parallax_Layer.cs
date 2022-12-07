using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [AddComponentMenu("I2/Parallax/I2Parallax 2D Layer")]
    [RequireComponent(typeof(RectTransform))]
    public class I2Parallax_Layer : MonoBehaviour, IParallaxLayer
    {
        [Range(0,130)] public float _Depth;
        public Vector2 _DepthFactor = Vector2.one;

        [Header("Constraint")]
        public bool _Constrain_Top;
        public bool _Constrain_Bottom;
        public bool _Constrain_Left;
        public bool _Constrain_Right;

        [Header("Expand")]
        [Range(0, 1)] public float _AutoExpand_Top;
        [Range(0, 1)] public float _AutoExpand_Bottom;
        [Range(0, 1)] public float _AutoExpand_Left;
        [Range(0, 1)] public float _AutoExpand_Right;

        RectTransform mTransform_Child, mTransform_Parent;
        Rect mConstrainRegion = Rect.MinMaxRect(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue);
        Vector2 mChildOffset;

        public virtual void OnEnable()
        {
            Initialize();
            I2Parallax_Manager.RegisterLayer(this);
        }

        public virtual void OnDisable()
        {
            I2Parallax_Manager.UnregisterLayer(this);
        }

        public void UpdateLayer( Vector2 dir )
        {
            if (mTransform_Parent == null)
            {
                Initialize();
                if (mTransform_Parent == null)
                    return;
            }

            float speed = Screen.dpi / 100.0f * _Depth;

            var offset = (Vector2)Vector2.Scale(dir, speed*_DepthFactor);

            if (_Constrain_Left && offset.x>mConstrainRegion.xMax)
                offset.x = mConstrainRegion.xMax;

            if (_Constrain_Right && offset.x < mConstrainRegion.xMin)
                offset.x = mConstrainRegion.xMin;

            if (_Constrain_Top && offset.y < mConstrainRegion.yMin)
                offset.y = mConstrainRegion.yMin;

            if (_Constrain_Bottom && offset.y > mConstrainRegion.yMax)
                offset.y = mConstrainRegion.yMax;

            mTransform_Child.localPosition = offset + mChildOffset;
        }

        void Initialize()
        {
            if (mTransform_Parent != null)
                return;

            mTransform_Child = transform as RectTransform;
            if (mTransform_Child.rect.width<=0 || mTransform_Child.rect.height<=0)
                return;

            // Create a parent with the same pos/rot/scale/...
            {
                int sibblingIdx = mTransform_Child.GetSiblingIndex();

                var parentGO = new GameObject("I2P_Layer ("+name+")", typeof(RectTransform));
                mTransform_Parent = parentGO.GetComponent<RectTransform>();

                mTransform_Parent.SetParent(mTransform_Child.parent);
                mTransform_Parent.SetSiblingIndex(sibblingIdx);

                mTransform_Parent.position = mTransform_Child.position;
                mTransform_Parent.rotation = mTransform_Child.rotation;
                mTransform_Parent.localScale = mTransform_Child.localScale;
                mTransform_Parent.anchorMax = mTransform_Child.anchorMax;
                mTransform_Parent.anchorMin = mTransform_Child.anchorMin;
                mTransform_Parent.offsetMax = mTransform_Child.offsetMax;
                mTransform_Parent.offsetMin = mTransform_Child.offsetMin;
                mTransform_Parent.anchoredPosition3D = mTransform_Child.anchoredPosition3D;
                mTransform_Parent.pivot = mTransform_Child.pivot;

                // Remove anchoring
                var size = mTransform_Child.rect.size;
                mTransform_Child.anchorMax = mTransform_Child.anchorMin = 0.5f * Vector2.one;
                mTransform_Child.sizeDelta = size;

                mTransform_Child.SetParent(mTransform_Parent, true);
            }



            // Expand and constraint
            AutoExpandChild();
            UpdateConstraint();
        }

        void AutoExpandChild()
        {
            if (_AutoExpand_Top <= float.Epsilon &&     _AutoExpand_Bottom <= float.Epsilon &&
                _AutoExpand_Left <= float.Epsilon &&    _AutoExpand_Right <= float.Epsilon)
            {
                return;
            }

            float speed = _Depth * Screen.dpi / 100.0f;

            float left      = _AutoExpand_Left * speed * _DepthFactor.x;
            float right     = _AutoExpand_Right * speed *  _DepthFactor.x;
            float top       = _AutoExpand_Top * speed * _DepthFactor.y;
            float bottom    = _AutoExpand_Bottom * speed * _DepthFactor.y;

            mChildOffset.x = 0.5f * (-left + right);
            mChildOffset.y = 0.5f * (-top + bottom);
            mTransform_Child.sizeDelta += new Vector2(left + right, top+bottom);
        }

        void UpdateConstraint()
        {
            var min = new Vector2(10000.0f, 10000.0f);
            var max = new Vector2(-10000.0f, -10000.0f);
            var superParent = mTransform_Parent.parent as RectTransform;

            if (superParent!=null && (_Constrain_Top || _Constrain_Bottom || _Constrain_Left || _Constrain_Right))
            {
                var corners = new Vector3[4];
                superParent.GetWorldCorners(corners);
                for (int i = 0; i < 4; ++i)
                {
                    var v = mTransform_Child.InverseTransformPoint(corners[i]);
                    if (v.x < min.x) min.x = v.x;
                    if (v.y < min.y) min.y = v.y;
                    if (v.x > max.x) max.x = v.x;
                    if (v.y > max.y) max.y = v.y;
                }

                var rectChild = mTransform_Child.rect;
                var rectParent = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
                min.x = min.y = -10000.0f;
                max.x = max.y = 10000.0f;

                if (_Constrain_Left && rectChild.xMax >= rectParent.xMax)
                    max.x = rectParent.xMin - rectChild.xMin;

                if (_Constrain_Right && rectChild.xMin <= rectParent.xMin)
                    min.x = rectParent.xMax - rectChild.xMax;

                if (_Constrain_Bottom && rectChild.yMax >= rectParent.yMax)
                    max.y = rectParent.yMin - rectChild.yMin;

                if (_Constrain_Top && rectChild.yMin <= rectParent.yMin)
                    min.y = rectParent.yMax - rectChild.yMax;
            }
            mConstrainRegion = Rect.MinMaxRect(min.x,min.y, max.x,max.y);
        }
    }
}