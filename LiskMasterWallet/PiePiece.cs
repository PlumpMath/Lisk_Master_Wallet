﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiskMasterWallet.Helpers;

namespace LiskMasterWallet
{
    /// <summary>
    ///     A pie piece shape
    /// </summary>
    public class PiePiece : Canvas
    {
        #region events

        public delegate void PiePieceClicked(PiePiece sender);

        //public event PiePieceClicked PiePieceClickedEvent;

        #endregion

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadius", typeof(double), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty WedgeAngleProperty =
            DependencyProperty.Register("WedgeAngle", typeof(double), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.Register("RotationAngle", typeof(double), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty CentreYProperty =
            DependencyProperty.Register("CentreY", typeof(double), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));

        public static readonly DependencyProperty CentreXProperty =
            DependencyProperty.Register("CentreX", typeof(double), typeof(PiePiece),
                new PropertyMetadata(OnDependencyPropertyChanged));


        public PiePiece()
        {
            AddPathToCanvas();
            //this.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(PiePiece_MouseLeftButtonUp);
        }

        /// <summary>
        ///     The radius of this pie piece
        /// </summary>
        public double Radius
        {
            get { return (double) GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        /// <summary>
        ///     The fill color of this pie piece
        /// </summary>
        public Brush Fill
        {
            get { return (Brush) GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        ///     The fill color of this pie piece
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush) GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        ///     The inner radius of this pie piece
        /// </summary>
        public double InnerRadius
        {
            get { return (double) GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        /// <summary>
        ///     The wedge angle of this pie piece in degrees
        /// </summary>
        public double WedgeAngle
        {
            get { return (double) GetValue(WedgeAngleProperty); }
            set { SetValue(WedgeAngleProperty, value); }
        }


        /// <summary>
        ///     The rotation, in degrees, from the Y axis vector of this pie piece.
        /// </summary>
        public double RotationAngle
        {
            get { return (double) GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        /// <summary>
        ///     The Y coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreY
        {
            get { return (double) GetValue(CentreYProperty); }
            set { SetValue(CentreYProperty, value); }
        }

        /// <summary>
        ///     The Y coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreX
        {
            get { return (double) GetValue(CentreXProperty); }
            set { SetValue(CentreXProperty, value); }
        }


        /// <summary>
        ///     The value that this pie piece represents.
        /// </summary>
        public double PieceValue { get; set; }

        private static void OnDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as PiePiece;
            source.AddPathToCanvas();
        }

        ///// <summary>
        ///// Handle left mouse button click event, hit testing to see if the pie piece was hit.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void PiePiece_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    IEnumerable<UIElement> hits = System.Windows.Media.VisualTreeHelper.FindElementsInHostCoordinates(e.GetPosition(null), this);

        //    Path path = (Path)hits.First(element => element is Path);
        //    if (path != null)
        //    {
        //        if (PiePieceClickedEvent != null)
        //        {
        //            PiePieceClickedEvent(this);
        //        }
        //    }
        //}

        private void AddPathToCanvas()
        {
            Children.Clear();

            var tooltip = (WedgeAngle/360.00).ToString("#0.##%");

            var path = ConstructPath();
            ToolTipService.SetToolTip(path, tooltip);
            Children.Add(path);
        }

        /// <summary>
        ///     Constructs a path that represents this pie segment
        /// </summary>
        /// <returns></returns>
        private Path ConstructPath()
        {
            if (WedgeAngle >= 360)
            {
                var path = new Path
                {
                    Fill = Fill,
                    Stroke = Stroke,
                    StrokeThickness = 1,
                    Data = new GeometryGroup
                    {
                        FillRule = FillRule.EvenOdd,
                        Children = new GeometryCollection
                        {
                            new EllipseGeometry
                            {
                                Center = new Point(CentreX, CentreY),
                                RadiusX = Radius,
                                RadiusY = Radius
                            },
                            new EllipseGeometry
                            {
                                Center = new Point(CentreX, CentreY),
                                RadiusX = InnerRadius,
                                RadiusY = InnerRadius
                            }
                        }
                    }
                };
                return path;
            }


            var startPoint = new Point(CentreX, CentreY);
            var p1 = AppHelpers.ComputeCartesianCoordinate(RotationAngle, InnerRadius);
            p1.Offset(CentreX, CentreY);
            var innerArcStartPoint = p1;
            var p2 = AppHelpers.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, InnerRadius);
            p2.Offset(CentreX, CentreY);
            var innerArcEndPoint = p2;
            var p3 = AppHelpers.ComputeCartesianCoordinate(RotationAngle, Radius);
            p3.Offset(CentreX, CentreY);
            var outerArcStartPoint = p3;
            var p4 = AppHelpers.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, Radius);
            p4.Offset(CentreX, CentreY);
            var outerArcEndPoint = p4;

            var largeArc = WedgeAngle > 180.0;
            var outerArcSize = new Size(Radius, Radius);
            var innerArcSize = new Size(InnerRadius, InnerRadius);

            var figure = new PathFigure
            {
                StartPoint = innerArcStartPoint,
                Segments = new PathSegmentCollection
                {
                    new LineSegment
                    {
                        Point = outerArcStartPoint
                    },
                    new ArcSegment
                    {
                        Point = outerArcEndPoint,
                        Size = outerArcSize,
                        IsLargeArc = largeArc,
                        SweepDirection = SweepDirection.Clockwise,
                        RotationAngle = 0
                    },
                    new LineSegment
                    {
                        Point = innerArcEndPoint
                    },
                    new ArcSegment
                    {
                        Point = innerArcStartPoint,
                        Size = innerArcSize,
                        IsLargeArc = largeArc,
                        SweepDirection = SweepDirection.Counterclockwise,
                        RotationAngle = 0
                    }
                }
            };

            return new Path
            {
                Fill = Fill,
                Stroke = Stroke,
                StrokeThickness = 1,
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection
                    {
                        figure
                    }
                }
            };
        }
    }
}