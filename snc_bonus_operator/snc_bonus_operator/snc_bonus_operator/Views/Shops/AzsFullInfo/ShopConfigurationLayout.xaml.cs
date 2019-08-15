using ssnc_bonus_operator;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace snc_bonus_operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShopConfigurationLayout : AbsoluteLayout
    {
        private int _columnsTotalNumber = 0;


        /// <summary>
        /// Список представлений всех пистолетов АЗС
        /// </summary>
        private Dictionary<int, List<Frame>> _nozzlesViews = new Dictionary<int, List<Frame>>();
        /// <summary>
        /// Список представлений колонок
        /// </summary>
        private Dictionary<int, Frame> _columnsViews = new Dictionary<int, Frame>();
        /// <summary>
        /// Список колонок
        /// </summary>
        private Dictionary<int, Column> _columns = new Dictionary<int, Column>();
        /// <summary>
        /// АЗС на которой заправляется клиент
        /// </summary>
        private ShopView _azs;
        /// <summary>
        /// Обновление информации об АЗС
        /// </summary>
        private AzsController _controller;

        public ShopConfigurationLayout ()
		{
			InitializeComponent ();
        }

        public void OnAppearing()
        {
            try
            {
                _controller.StartMonitor();
                _controller.UpdateUpdateColumns += UpdateColumns;
                _controller.ErrorLoading += ErrorLoading;
                IsVisible = true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public void OnDisappearing()
        {
            try
            {
                if (_controller != null)
                    _controller.UpdateUpdateColumns -= UpdateColumns;
                IsVisible = false;
                _controller?.StopMonitor();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void ErrorLoading(Column column)
        {
            if (IsVisible)
            {
                
            }
        }
        private void UpdateColumns(Column column)
        {
            if (column == null)
            {
                return;
            }
            try
            {
                mainLayout.IsVisible = true;
                emptyFrame.IsVisible = false;
                EndLoading();

                // Добавляем колонку, если не найдена
                Frame columnFrame;
                if (!_columns.ContainsKey(column.Id))
                {
                    _columns.Add(column.Id, column);
                    columnFrame = AddNewColumnFrame(column);
                }
                else
                {
                    columnFrame = _columnsViews[column.Id];
                }

                // Обновляем пистолеты в колонке
                Grid nozzlesGrid = (Grid)(columnFrame).Content;
                if (nozzlesGrid.ClassId == "grid" + column.Id)
                {
                    nozzlesGrid.Children.Clear();
                    if (!_nozzlesViews.ContainsKey(column.Id))
                        _nozzlesViews.Add(column.Id, new List<Frame>());
                    _nozzlesViews[column.Id].Clear();
                    var index = 0;
                    foreach (var nozzle in column.GetHoses())
                    {
                        var frame = CreateNozzleButton(nozzle);
                        _nozzlesViews[column.Id].Add(frame);
                        nozzlesGrid.Children.Add(frame, index % 2, index / 2);
                        ++index;
                    }
                }

                // Устанавливаем доступность пистолетов колонки по состоянию
                if (column.State == PumpStateEnum.PUMP_STATE_FREE)
                {
                    UnlockTrk(column.Id, -1);
                }
                else
                {
                    LockTrk(column.Id, -1);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }


        #region methods

        /// <summary>
        /// Установить контекст
        /// </summary>
        /// <param name="selected"></param>
        public void SetContext(ShopView selected)
        {
            StartLoading();
            _controller = new AzsController(this, selected);
        }

        /// <summary>
        /// Отображение состояния загрузки
        /// </summary>
        private void StartLoading()
        {
            backgroundDark.BackgroundColor = new Color(0, 0, 0, 0.5);
            backgroundDark.IsVisible = true;

            mainLayout.IsEnabled = false;
            IndicatorLayout.Start();
        }
        /// <summary>
        /// Отображение завершения загрузки
        /// </summary>
        private void EndLoading()
        {
            backgroundDark.IsVisible = false;

            mainLayout.IsEnabled = true;
            IndicatorLayout.Stop();
        }

        /// <summary>
        /// Сгенерировать фрейм для одной колонки с пистолетами
        /// </summary>
        /// <param name="column">Колонка</param>
        /// <returns>Фрейм колонки</returns>
        private Frame AddNewColumnFrame(Column column)
        {
            // Новый ряд для таблицы колонок
            gridColumns.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Auto) });

            // Отображение занятой колонки
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 5
            };
            var emptyColumn = new Frame { IsVisible = true, Opacity = 0.5 };
            var empty = new StackLayout { Padding = 12 };
            empty.Children.Add(new FFImageLoading.Forms.CachedImage { Source = "emptyAzs.png", HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand });
            empty.Children.Add(new Label { Text = "Нет Данных", Style = (Style)App.Current.Resources["UsualLabelStyle"], HorizontalOptions = LayoutOptions.CenterAndExpand, FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) });
            emptyColumn.Content = empty;

            var nozzlesGrid = new Grid { ClassId = "grid" + column.Id };
            nozzlesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            nozzlesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
            nozzlesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Auto) });
            nozzlesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Auto) });
            nozzlesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Auto) });
            nozzlesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60, GridUnitType.Auto) });

            stack.Children.Add(emptyColumn);
            stack.Children.Add(nozzlesGrid);
            var columnFrame = new Frame { ClassId = column.Name, Content = nozzlesGrid };
            gridColumns.Children.Add(columnFrame, 0, _columnsTotalNumber);
            _columnsViews.Add(column.Id, columnFrame);
            _columnsTotalNumber++;
            return columnFrame;
        }
        /// <summary>
        /// Сгенерировать кнопку для пистолета
        /// </summary>
        /// <param name="nozzle">Пистолет</param>
        /// <returns>Фрэйм пистолета</returns>
        private Frame CreateNozzleButton(Nozzle nozzle)
        {
            var frame = new Frame()
            {
                Style = (Style)App.Current.Resources["UsualFrameStyle"],
                ClassId = nozzle.Id.ToString(),
                Padding = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                CornerRadius = 10,
                BackgroundColor = (Color)App.Current.Resources["SelectionColor"]
            };
            var miniFrame = new Frame()
            {
                Style = (Style)App.Current.Resources["UsualFrameStyle"],
                Margin = 0,
                Padding = 1,
                HasShadow = false
            };
            var gesture = new TapGestureRecognizer();
            frame.GestureRecognizers.Add(gesture);
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 5
            };
            var prefix = GetFuelPrefix(nozzle.Id);
            var topLbl = new Label
            {
                Text = prefix,
                Style = (Style)App.Current.Resources["BoldUsualLabelStyle"],
                HorizontalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            var bottomLbl = new Label
            {
                Text = nozzle.FuelName,
                Style = (Style)App.Current.Resources["BoldUsualLabelStyle"],
                HorizontalTextAlignment = TextAlignment.Center,
            };

            stack.Children.Add(topLbl);
            stack.Children.Add(bottomLbl);
            miniFrame.Content = stack;
            frame.Content = miniFrame;
            return frame;
        }
        /// <summary>
        /// Установка визуального оформления на кнопку
        /// </summary>
        /// <param name="col">Кнопка</param>
        /// <param name="column">Колонка</param>
        private void SetStatusCol(Button col, Column column)
        {
            //if (column.State == PumpStateEnum.PUMP_STATE_FREE)
            //{
            //    col.FontAttributes = FontAttributes.Bold;
            //    col.BorderColor = (Color)App.Current.Resources["MainColor"];
            //    if (_column.Id == column.Id)
            //    {
            //        UnlockTrk();
            //    }
            //}
            //else
            //{
            //    col.FontAttributes = FontAttributes.None;
            //    col.BorderColor = (Color)App.Current.Resources["BackgroundColors"];
            //    if (_column.Id == column.Id)
            //    {
            //        BlockTrk();
            //    }
            //}

        }
        /// <summary>
        /// Заблокировать пистолеты (заняты)
        /// </summary>
        /// <param name="columnId">Номер колонки</param>
        /// <param name="nozzleId">Номер пистолета (-1 на все)</param>
        private void UnlockTrk(int columnId, int nozzleId = -1)
        {
            Button button = null;
            if (nozzleId == -1)
            {
                foreach (Frame nozzle in _nozzlesViews[columnId])
                {
                    nozzle.BackgroundColor = (Color)App.Current.Resources["MainColor"];
                }
            }
            else
            {
                var nozzleFrame = FindNozzle(_columns[columnId], nozzleId);
                Grid nozzlesGrid = (Grid)nozzleFrame.Content;
                button = (Button)nozzlesGrid.Children.FirstOrDefault(x => x.ClassId == nozzleId.ToString());
                button.FontAttributes = FontAttributes.Bold;
                button.BorderColor = (Color)App.Current.Resources["MainColor"];
            }
        }
        /// <summary>
        /// Разблокировать пистолеты (свободные)
        /// </summary>
        /// <param name="columnId">Номер колонки</param>
        /// <param name="nozzleId">Номер пистолета (-1 на все)</param>
        private void LockTrk(int columnId, int nozzleId = -1)
        {
            Button button = null;
            if (nozzleId == -1)
            {
                foreach (var nozzle in _nozzlesViews[columnId])
                {
                    nozzle.BackgroundColor = (Color)App.Current.Resources["BackgroundColors"];
                }
            }
            else
            {
                var nozzleFrame = FindNozzle(_columns[columnId], nozzleId);
                Grid nozzlesGrid = (Grid)nozzleFrame.Content;
                button = (Button)nozzlesGrid.Children.FirstOrDefault(x => x.ClassId == nozzleId.ToString());
                button.FontAttributes = FontAttributes.None;
                button.BorderColor = (Color)App.Current.Resources["BackgroundColors"];
            }
        }
        /// <summary>
        /// Найти фрэйм колонки
        /// </summary>
        /// <param name="col">Номер колонки</param>
        /// <returns></returns>
        private Frame FindCollumn(int col)
        {
            var res = gridColumns.Children.FirstOrDefault(x => x.ClassId == col.ToString());
            if (res is Frame)
                return (Frame)res;
            return null;
        }
        /// <summary>
        /// Найти фрэйм пистолета
        /// </summary>
        /// <param name="col">Колонка</param>
        /// <param name="nozzleId">Номер пистолета</param>
        /// <returns></returns>
        private Frame FindNozzle(Column col, int nozzleId)
        {
            var columnFrame = FindCollumn(col.Id);
            if (columnFrame == null)
                return null;
            if (columnFrame.Content is Grid)
            {
                Grid nozzlesGrid = (Grid)columnFrame.Content;
                var res = nozzlesGrid.Children.FirstOrDefault(x => x.ClassId == nozzleId.ToString());
                if (res is Frame)
                    return (Frame)res;
            }
            return null;
        }
        /// <summary>
        /// Префикс с номером пистолета
        /// </summary>
        /// <param name="nozzleId">Номер пистолета</param>
        /// <returns>Форматированный префикс номера пистолета</returns>
        private string GetFuelPrefix(int nozzleId)
        {
            string res = string.Format("№ {0} ", nozzleId);
            return res;
        }
        /// <summary>
        /// Найти колонку по номеру пистолета
        /// </summary>
        /// <param name="nozzleId">Номер пистолета</param>
        /// <returns>Класс колонки</returns>
        private Column GetColumnByNozzle(int nozzleId)
        {
            var cKey = -1;
            foreach (var nv in _nozzlesViews)
            {
                if (nv.Value.FirstOrDefault(x => x.ClassId == nozzleId.ToString()) != null)
                    cKey = nv.Key;
            }
            if (cKey == -1)
                return null;
            return _columns[cKey];
        }
        #endregion
    }
}