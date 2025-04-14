using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;


namespace Model
{
    public class CanvasModel
    {
        private TableModel _tableModel;
        private Canvas _canvas;
        public CanvasModel(TableModel t)
        {
            this.tableModel = t;
            this._canvas = new Canvas()
            {
                Width = tableModel.Table.width,
                Height = tableModel.Table.height,
                Background = Brushes.LightGreen
            };
        }

        public TableModel tableModel
        {
            get { return _tableModel; }
            set { _tableModel = value; }
        }

        public void addObject(UIElement obj)
        {
            _canvas.Children.Add(obj);
        }

        public Canvas canvas
        {
            get { return _canvas; }
        }
    }
}
