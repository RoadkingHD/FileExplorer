using System;
using System.Windows.Forms;

namespace FileExplorer.Controls
{
    /// <summary>
    /// A TreeView that raises an event whenever the view is scrolled, so two
    /// tree views can be kept in synchronized scrolling.
    /// </summary>
    public class ScrollableTreeView : TreeView
    {
        private const int WmHScroll = 0x0114;
        private const int WmVScroll = 0x0115;
        private const int WmMouseWheel = 0x020A;
        private const int WmMouseHWheel = 0x020E;

        public event EventHandler ScrollMoved;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WmVScroll || m.Msg == WmHScroll
                || m.Msg == WmMouseWheel || m.Msg == WmMouseHWheel)
            {
                OnScrollMoved(EventArgs.Empty);
            }
        }

        protected virtual void OnScrollMoved(EventArgs e)
        {
            EventHandler handler = ScrollMoved;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
