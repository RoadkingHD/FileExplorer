using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FileExplorer.Controls
{
    /// <summary>
    /// Narrow vertical strip shown next to a diff text box. It paints a
    /// colored tick at the vertical position of every difference block in
    /// the document (like the overview marker bar of a code editor) and
    /// raises <see cref="JumpRequested"/> when the user clicks a mark.
    /// </summary>
    public class DiffMarkerStrip : Panel
    {
        /// <summary>A single difference marker.</summary>
        public class DiffMark
        {
            /// <summary>Zero-based rendered line index of the difference.</summary>
            public int Line { get; set; }

            /// <summary>Tick color.</summary>
            public Color Color { get; set; }
        }

        /// <summary>Event data for a click on a marker.</summary>
        public class MarkerJumpEventArgs : EventArgs
        {
            public MarkerJumpEventArgs(int line)
            {
                Line = line;
            }

            /// <summary>Zero-based rendered line index the user clicked at.</summary>
            public int Line { get; private set; }
        }

        private readonly List<DiffMark> _marks = new List<DiffMark>();
        private int _totalLines;

        /// <summary>Raised when the user clicks the strip to jump to a line.</summary>
        public event EventHandler<MarkerJumpEventArgs> JumpRequested;

        public DiffMarkerStrip()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw,
                true);

            Dock = DockStyle.Right;
            Width = 12;
            BackColor = SystemColors.Control;
            Cursor = Cursors.Hand;
            TabStop = false;
        }

        /// <summary>Replaces all markers. <paramref name="totalLines"/> is the number of rendered lines in the owner box.</summary>
        public void SetMarks(IEnumerable<DiffMark> marks, int totalLines)
        {
            _marks.Clear();

            if (marks != null)
            {
                _marks.AddRange(marks);
            }

            _totalLines = totalLines;
            Invalidate();
        }

        /// <summary>Removes all markers.</summary>
        public void ClearMarks()
        {
            _marks.Clear();
            _totalLines = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Track background.
            using (var track = new SolidBrush(Color.FromArgb(226, 226, 226)))
            {
                e.Graphics.FillRectangle(track, 0, 0, Width, Height);
            }

            if (_totalLines <= 0 || _marks.Count == 0 || Height <= 4)
            {
                return;
            }

            float scale = Height / (float)_totalLines;
            int tickHeight = Math.Max(2, (int)Math.Ceiling(scale));

            foreach (DiffMark mark in _marks)
            {
                int y = (int)(mark.Line * scale);

                if (y > Height - 1)
                {
                    y = Height - 1;
                }

                using (var brush = new SolidBrush(mark.Color))
                {
                    e.Graphics.FillRectangle(brush, 1, y, Width - 3, tickHeight);
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left || _totalLines <= 0 || Height <= 0)
            {
                return;
            }

            int line = (int)((long)e.Y * _totalLines / Height);

            if (line < 0)
            {
                line = 0;
            }
            else if (line >= _totalLines)
            {
                line = _totalLines - 1;
            }

            EventHandler<MarkerJumpEventArgs> handler = JumpRequested;

            if (handler != null)
            {
                handler(this, new MarkerJumpEventArgs(line));
            }
        }
    }
}
