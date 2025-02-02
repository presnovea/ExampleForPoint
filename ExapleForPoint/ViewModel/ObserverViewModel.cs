﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

using ExapleForPoint.Modelling;

namespace ExapleForPoint.ViewModel
{
    /// <summary>
    /// Модель-представление окна Обозревателя 
    /// </summary>
    public class ObserverViewModel : INotifyPropertyChanged
    {
        //--------Объявление свойств, делегатов и событий-----------------------------
        private PointExampleContext exampleContext;
        private ISessionContext currentSessionContext;

        private List<Customers> customers;
        private List<Orders> orders;
        private Customers selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sessionContext"></param>
        public ObserverViewModel(ISessionContext sessionContext)
        {
            currentSessionContext = sessionContext;
            currentSessionContext.PropertyChanged += OnSessionContextPropertyChanged;
        }



        //---------Свойства, к которым происходит привязка View--------------------------
        public List<Customers> Customers
        {
            get { return customers; }
            set { customers = value;
                OnPropertyChanged("Customers");
            }
        }

        public List<Orders> Orders
        {
            get { return orders; }
            set { orders = value;
                OnPropertyChanged("Orders");
            }
        }

        public PointExampleContext ExampleContext
        {
            get { return exampleContext; }
            set { exampleContext = value;}
        }

        public Customers SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                GetOrdersByCustomer(ExampleContext);
                OnPropertyChanged("SelectedItem");
            }
        }



        //----------Методы------------------------------------------------------------
        /// <summary>
        /// Данный обработчик получает обновленный контекст сессии всего приложения 
        /// и инициализирует общее обновление данных в представлении
        /// </summary>
        private void OnSessionContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SessionExampleContext")
            {
                this.exampleContext = currentSessionContext.SessionExampleContext;
                Customers = exampleContext.customers.ToList<Customers>();
            }
        }

        /// <summary>
        /// Асинхронный метод получения выборки заказов относительно Заказчика
        /// </summary>
        /// <param name="context"></param>
        private async void GetOrdersByCustomer (PointExampleContext context)
        {
            await Task.Run(() =>
           {
               try { Orders = exampleContext.GetOrdersByCustomer(selectedItem.ID); }
               catch (Exception ex) { MessageBox.Show(ex.Message); }
           });
        }
    }
}
