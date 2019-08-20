using SnakeWithBrain.Brain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SnakeWithBrain.Brain
{
    public class NetworkModelView : FrameworkElement
    {
        private NetworkModel _model;

        public NetworkModelView(NetworkModel model)
        {
            _model = model;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(Brushes.Black, new Pen(), new Rect(new Point(0, 0), new Size(ActualWidth, ActualHeight)));

            var drawer = new ModelDrawer(_model, drawingContext, ActualWidth, ActualHeight);

            drawer.Draw();
        }

        private class ModelDrawer
        {
            private readonly NetworkModel _model;
            private readonly DrawingContext _drawingContext;
            private readonly double _width;
            private readonly double _height;

            private readonly double _neuronSize = 100;
            private readonly double _neuronPadding = 30;
            private readonly double _layerPadding = 100;

            public ModelDrawer(NetworkModel model, DrawingContext drawingContext, double width, double height)
            {
                _model = model;
                _drawingContext = drawingContext;
                _width = width;
                _height = height;

            }

            public void Draw()
            {
                var brushes = new Brush[]
                {
                    Brushes.Blue,
                    Brushes.Red,
                    Brushes.Green
                };

               var layerWidth = _layerPadding + _neuronSize + _layerPadding;

                for (int i = 0; i < _model.Layers.Count; i++)
                {
                    var layer = _model.Layers[i];

                    //_drawingContext.DrawRectangle(brushes[i], new Pen(), new Rect(new Point(i * layerWidth, 0), new Size(layerWidth, _height)));

                    var topPadding = (_height-(_neuronSize * layer.Neurons.Count + (layer.Neurons.Count - 1) * _neuronPadding))/2;

                    for (int j = 0; j < layer.Neurons.Count; j++)
                    {
                        var neuron = layer.Neurons[j];

                        DrawNeuron(neuron, new Rect(new Point(_layerPadding + i * layerWidth, topPadding+j * (_neuronSize+ _neuronPadding)), new Size(_neuronSize, _neuronSize)));
                    }
                }
            }

            private void DrawNeuron(Neuron neuron, Rect rect)
            {
                _drawingContext.DrawRectangle(Brushes.White, new Pen(), rect);
            }
        }
    }
}
