using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using PLANET3.Data;
using PLANET3.Models;
using Terminal.Gui;
using NStack;
using static System.Net.Mime.MediaTypeNames;

namespace PLANET3
{
    public class GUI : Window
    {
        private Planet3Context _context;
        public string selectedCustomer;
        private int _currrentClientId;

        public GUI()
        {
            _context = new Planet3Context();
            ShowClientList();
        }

        private void ShowChosenClientPage()
        {
            string clientName = _context.Customers.Where(i => i.Id == _currrentClientId).First().Name;
            RemoveAll();
            Title = clientName;

            var buttonbackToClientList = new Button("go back to client list")
            {
                X = Pos.Center(),
                Y = 2
            };

            buttonbackToClientList.Clicked += () =>
            {
                ShowClientList();
            };
            Add(buttonbackToClientList);
            var buttonShowItems = new Button("show items")
            {
                X = Pos.Right(buttonbackToClientList),
                Y = 2
            };

            buttonShowItems.Clicked += () =>
            {
                ShowItemList();
            };
            Add(buttonShowItems);

            List<string> orders = new List<string>();
            foreach (var order in _context.Order.Where(o => o.CustomerId == _currrentClientId).ToList())
            {

                string s = "|id: " + order.Id + "|items: " + order.AmountOfItems() + "|price: "+ order.TotalPrice() + "|completed: " + order.IsCompleted;
                orders.Add(s);
            }

            var orderList = new ListView(orders)
            {
                X = 0,
                Y = Pos.Bottom(buttonShowItems),
                Width = Dim.Fill(),
                Height = Dim.Fill(2)
            };

            //orderList.SelectedItemChanged += CustomerChosen;
            Add(orderList);
        }
        private void CustomerChosen(ListViewItemEventArgs args)
        {
            string[] words = args.Value.ToString().Split(' ');
            int idx = int.Parse(words[0]);
            _currrentClientId = idx;
            ShowChosenClientPage();
            
        }

        private void ShowClientList()
        {
            RemoveAll();

            Title = "List of Clients";
            var textFilter = new TextField()
            {
                X = 10,
                Y = 2,
                Width = 10,
                Text = "filter"
            };
            Add(textFilter);
            var buttonFilter = new Button("filter clients")
            {
                X = Pos.Right(textFilter),
                Y = 2
            };

            List<string> customers = new List<string>();
            foreach (var customer in _context.Customers.ToList())
            {
                string s = customer.Id + " " + customer.Name;
                customers.Add(s);
            }

            var clientList = new ListView(customers)
            {
                X = 0,
                Y = Pos.Bottom(buttonFilter),
                Width = Dim.Fill(),
                Height = Dim.Fill(2)
            };

            clientList.SelectedItemChanged += CustomerChosen;
            Add(clientList);

            buttonFilter.Clicked += () =>
            {
                var filteredList = _context.Customers.Where(i => i.Name.StartsWith(textFilter.Text.ToString())).ToList();
                List<string> filteredNames = new List<string>();
                foreach (var customer in filteredList)
                {
                    string s = customer.Id + " " + customer.Name;
                    filteredNames.Add(s);
                }
                clientList.SetSource(new ArrayList(filteredNames));
            };
            Add(buttonFilter);
        }

        private void CreateNewItem()
        {
            RemoveAll();

            Title = "create new item";

            var textName = new TextField()
            {
                X = Pos.Center() ,
                Y = Pos.Center(),
                Width = 10,
                Text = "name"
            };
            Add(textName);

            var textPrice = new TextField()
            {
                X = Pos.Right(textName),
                Y = Pos.Center(),
                Width = 10,
                Text = "price"
            };
            Add(textPrice);

            var buttonCreate = new Button("create item")
            {
                X = Pos.Center() - 10,
                Y = Pos.Bottom(textPrice)
            };

            buttonCreate.Clicked += () =>
            {
                int price;

                bool success = int.TryParse(textPrice.Text.ToString(), out price);
                if (success)
                {
                    string name = textName.Text.ToString();
                    Item item = new Item()
                    {
                        Name = name,
                        Price = price
                    };

                    _context.Add(item);
                    _context.SaveChanges();
                    _context.SaveChanges();
                    string msg = name + " created. Price is " + price;
                    MessageBox.Query(name, msg, "OK");
                }

            };
            Add(buttonCreate);

            var buttonBack = new Button("go back to list")
            {
                X = Pos.Center() + 10,
                Y = Pos.Bottom(textPrice)
            };

            buttonBack.Clicked += () =>
            {
                ShowItemList();

            };
            Add(buttonBack);

        }
        private void ShowItemList()
        {
            RemoveAll();

            Title = "List of Items";


            var buttonNew = new Button("new item")
            {
                X = Pos.Center(),
                Y = 2,
            };

            buttonNew.Clicked += () =>
            {
                CreateNewItem();

            };
            Add(buttonNew);

            var buttonbackToClient = new Button("go back to client page")
            {
                X = Pos.Right(buttonNew),
                Y = 2
            };

            buttonbackToClient.Clicked += () =>
            {
                ShowChosenClientPage();
            };
            Add(buttonbackToClient);

            List<string> items = new List<string>();
            foreach (var item in _context.Item.ToList())
            {
                string s = "|id: " + item.Id + " |name: " + item.Name + " |price: " + item.Price + " |available amount: " + item.Stock;
                items.Add(s);
            }

            var itemtList = new ListView(items)
            {
                X = 0,
                Y = Pos.Bottom(buttonNew),
                Width = Dim.Fill(),
                Height = Dim.Fill(2)
            };

            itemtList.SelectedItemChanged += itemManagement;
            Add(itemtList);
        }
        private void itemManagement(ListViewItemEventArgs args)
        {
            string[] words = args.Value.ToString().Split(' ');
            int idx = int.Parse(words[1]);
            Item item = _context.Item.Where(i => i.Id == idx).First();
            RemoveAll();
            string itemName = words[3];
            Title = itemName;

            var textAmount = new TextField( 10,  10, 100, "amount");
            Add(textAmount);

            var buttonAdd = new Button("add items")
            {
                X = Pos.Center() - 10,
                Y = Pos.Center(),
            };

            buttonAdd.Clicked += () =>
            {
                int amount;

                bool success = int.TryParse(textAmount.Text.ToString(), out amount);
                if (success)
                {
                    amount += item.Stock;
                    item.Stock = amount;
                    _context.SaveChanges();
                    string msg = textAmount.Text.ToString() + " items added. Current number of " + itemName + " is " +
                                 amount;
                    MessageBox.Query(itemName, msg, "OK");
                }

            };
            Add(buttonAdd);
            var buttonBack = new Button("go back to list")
            {
                X = Pos.Center() + 10,
                Y = Pos.Center(),
            };

            buttonBack.Clicked += () =>
            {
                ShowItemList();

            };
            Add(buttonBack);
        }

        // private void CustomerChosen(ListViewItemEventArgs args)
        // {
        //
        // }
        // {
        //     string[] words = args.Value.ToString().Split(' ');
        //     int idx = int.Parse(words[0]);
        //
        //     RemoveAll();
        //     Title = words[1];
        //
        //     List<string> orders = new List<string>();
        //     foreach (var order in _context.Order.Where(i => i. == idx).ToList())
        //     {
        //         string s = order.Name;
        //         orders.Add(s);
        //     }
        //
        //     var orderListView = new ListView(orders)
        //     {
        //         X = 0,
        //         Y = 2,
        //         Width = Dim.Fill(),
        //         Height = Dim.Fill(2)
        //     };
        //
        //
        //     Add(orderListView);
        //     MediaTypeNames.Application.Top.SetNeedsDisplay();
        // }
        // // private void newAction(ListViewItemEventArgs args)
        // // {
        // //     string[] words = args.Value.ToString().Split(' ');
        // //     int idx = int.Parse(words[0]);
        // //
        // //     RemoveAll();
        // //     Title = words[1];
        // //
        // //     List<string> orders = new List<string>();
        // //     foreach (var order in _context.Order.Where(i => i. == idx).ToList())
        // //     {
        // //         string s = order.Name;
        // //         orders.Add(s);
        // //     }
        // //
        // //     var orderListView = new ListView(orders)
        // //     {
        // //         X = 0,
        // //         Y = 2,
        // //         Width = Dim.Fill(),
        // //         Height = Dim.Fill(2)
        // //     };
        // //
        // //
        // //     Add(orderListView);
        // //     MediaTypeNames.Application.Top.SetNeedsDisplay();
        // // }
    }
}
