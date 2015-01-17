using Android.Views;
using Android.Widget;
using System;

namespace SharpShoppingList.Helpers
{
    public class MultiChoiceModeListener : Java.Lang.Object, AbsListView.IMultiChoiceModeListener
    {
        private Func<ActionMode, IMenuItem, bool> _onActionItemClicked;
        private Func<ActionMode, IMenu, bool> _onCreateActionMode;
        private Action<ActionMode> _onDestroyActionMode;
        private Action<ActionMode, int, long, bool> _onItemCheckedStateChanged;
        private Func<ActionMode, IMenu, bool> _onPrepareActionMode;

        void AbsListView.IMultiChoiceModeListener.OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
        {
            if (_onItemCheckedStateChanged != null)
                _onItemCheckedStateChanged(mode, position, id, isChecked);
        }

        bool ActionMode.ICallback.OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            if (_onActionItemClicked == null)
                return false;

            return _onActionItemClicked(mode, item);
        }

        bool ActionMode.ICallback.OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            if (_onCreateActionMode == null)
                return false;

            return _onCreateActionMode(mode, menu);
        }

        void ActionMode.ICallback.OnDestroyActionMode(ActionMode mode)
        {
            if (_onDestroyActionMode != null)
                _onDestroyActionMode(mode);
        }

        bool ActionMode.ICallback.OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            if (_onPrepareActionMode == null)
                return false;

            return _onPrepareActionMode(mode, menu);
        }

        public MultiChoiceModeListener OnActionItemClicked(Func<ActionMode, IMenuItem, bool> func)
        {
            _onActionItemClicked = func;
            return this;
        }

        public MultiChoiceModeListener OnCreateActionMode(Func<ActionMode, IMenu, bool> func)
        {
            _onCreateActionMode = func;
            return this;
        }

        public MultiChoiceModeListener OnDestroyActionMode(Action<ActionMode> action)
        {
            _onDestroyActionMode = action;
            return this;
        }

        public MultiChoiceModeListener OnItemCheckedStateChanged(Action<ActionMode, int, long, bool> action)
        {
            _onItemCheckedStateChanged = action;
            return this;
        }

        public MultiChoiceModeListener OnPrepareActionMode(Func<ActionMode, IMenu, bool> func)
        {
            _onPrepareActionMode = func;
            return this;
        }
    }
}