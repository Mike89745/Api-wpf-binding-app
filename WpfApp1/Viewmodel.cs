using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
namespace WpfApp1
{
    class Viewmodel : INotifyPropertyChanged
    {
        public API api = new API();
        private User _user;
        private List<User> _users;
        private List<Order> _orders;
        private List<Item> _items;
        private List<Item> _OrderItems = new List<Item>();
        private string _itemName;
        private bool _itemNameCheck;
        private string _itemPrice;
        private bool _itemPriceCheck;
        private string _itemDescription;
        private bool _itemDescriptionCheck;
        private string _responseMsg;
        private string _Username;
        private string _UserPassword;
        private Item _selectedItem;
        private Item _OrderSelectedItem;
        private Order _ordersSelectedItem;
        public RelayCommand RelayItemsSelectionChanged { get; private set; }
        public RelayCommand RelayCreateItem { get; private set; }
        public RelayCommand RelayGetItems { get; private set; }
        public RelayCommand RelayUpdateItem { get; private set; }
        public RelayCommand RelayDeleteItem { get; private set; }
        public RelayCommand RelayAddItemToOrder { get; private set; }
        public RelayCommand RelayCreateOrder { get; private set; }
        public RelayCommand RelayRemoveItemFromOrder { get; private set; }
        public RelayCommand RelayLoginUser { get; private set; }
        public RelayCommand RelayDeteleUserOrder { get; private set; }
        public Viewmodel()
        {
            getItems();
            /*foreach (var item in items)
            {
                Debug.WriteLine(item.name);
            }*/
            RelayGetItems = new RelayCommand(this.getItems);
            RelayCreateItem = new RelayCommand(this.createItem);
            RelayUpdateItem = new RelayCommand(this.updateItem);
            RelayDeleteItem = new RelayCommand(this.deleteItem);
            RelayAddItemToOrder = new RelayCommand(this.addItemToOrderList);
            RelayCreateOrder = new RelayCommand(this.createOrder);
            RelayRemoveItemFromOrder = new RelayCommand(this.RemoveItemFromOrderList);
            RelayLoginUser = new RelayCommand(this.LoginUser);
            RelayDeteleUserOrder = new RelayCommand(this.deleteOrder);
        }
        public string responseMsg
        {
            get => _responseMsg;
            set
            {
                _responseMsg = value;
                OnPropertyChanged("responseMsg");
            }
        }
        public User user {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged("user");
            }
        }
        public List<User> users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged("users");
            }
        }
        public List<Order> orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged("orders");
            }
        }
        public List<Item> items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged("items");
            }
        }
        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    itemName = _selectedItem.name;
                    itemPrice = _selectedItem.price.ToString();
                    itemDescription = _selectedItem.description;
                }

                OnPropertyChanged("SelectedItem");
            }
        }
        public Order ordersSelectedItem
        {
            get => _ordersSelectedItem;
            set
            {
                _ordersSelectedItem = value;
                OnPropertyChanged("ordersSelectedItem");
            }
        }
        public Item OrderSelectedItem
        {
            get => _OrderSelectedItem;
            set
            {
                _OrderSelectedItem = value;
                OnPropertyChanged("OrderSelectedItem");
            }
        }
        public List<Item> OrderItems
        {
            get => _OrderItems;
            set
            {
                _OrderItems = value;
                OnPropertyChanged("OrderItems");
            }
        }
        public string itemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                CheckItem();
                OnPropertyChanged("itemName");
            }
        }
        public string itemPrice
        {
            get => _itemPrice;
            set
            {
                _itemPrice = value;

                CheckItem();
                OnPropertyChanged("itemPrice");
            }
        }
        public string itemDescription
        {
            get => _itemDescription;
            set
            {
                _itemDescription = value;
                CheckItem();
                OnPropertyChanged("itemDescription");
            }
        }
        public bool itemNameCheck
        {
            get => _itemNameCheck;
            set
            {
                _itemNameCheck = value;
                OnPropertyChanged("itemNameCheck");
            }
        }
        public bool itemPriceCheck
        {
            get => _itemPriceCheck;
            set
            {
                _itemPriceCheck = value;
                OnPropertyChanged("itemPriceCheck");
            }
        }
        public bool itemDescriptionCheck
        {
            get => _itemDescriptionCheck;
            set
            {
                _itemDescriptionCheck = value;
                OnPropertyChanged("itemDescriptionCheck");
            }
        }
        public string Username
        {
            get => _Username;
            set
            {
                _Username = value;
                OnPropertyChanged("Username");
            }
        }
        public string UserPassword
        {
            get => _UserPassword;
            set
            {
                _UserPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }
        public void ItemsSelectionChanged(object sender)
        {
            var Newitem = (ListBox)sender;
            var Itemsasd = (Item)Newitem.SelectedItem;
        }
        public void LoginUser()
        {
            _user = api.loginUser(_Username, _UserPassword);
            getUserOrders();
        }
        public void addItemToOrderList()
        {
            _OrderItems.Add(_selectedItem);
        }
        public void RemoveItemFromOrderList()
        {
            _OrderItems.Remove(_OrderSelectedItem);
        }
        public void createUser(string nick, string password, string email)
        {
            api.addUser(nick, password, email);
        }
        public async void setUser(string ID)
        {
            _user = await api.GetUser(ID);
        }
        public async void getUsers()
        {
            _users = await api.GetAllUsers();
        }
        public void updateUser(User user, string password)
        {
            api.updateUser(user, password);
        }
        public async void deleteUser(User user)
        {
            responseMsg = await api.deleteUser(user.ID);
        }
        public void createOrder()
        {
            api.addOrder(user.ID, OrderItems);
        }
        public async void getUserOrders()
        {
            orders = await api.getUserOrders(_user.ID);
        }
        public async void getAllOrders()
        {
            orders = await api.getAllOrders();
        }
        public void updateOrder(Order order)
        {
            //api.updateOrder(order);
        }
        public async void deleteOrder()
        {
            responseMsg = await api.deleteOrder(ordersSelectedItem.ID);
            for (int i = 0; i < _orders.Count; i++)
            {
                if(_orders[i].ID == ordersSelectedItem.ID)
                {
                    _orders.RemoveAt(i);
                    break;
                }
            }
        }
        public async void getItems()
        {
            items = await api.getAllItems();
        }
        public void CheckItem()
        {
            if (itemPrice != null)
            {
                if (Regex.IsMatch(itemPrice, "^[0-9]+$"))
                {
                    itemPriceCheck = true;
                }
                else
                {
                    itemPriceCheck = false;
                }
            }
            
            itemNameCheck = true;
            itemDescriptionCheck = true;
        }
        public async void createItem()
        {
            if(itemPriceCheck && itemNameCheck && itemDescriptionCheck)
            {
                Item item = new Item();
                item.name = itemName;
                item.price = int.Parse(itemPrice);
                item.description = itemDescription;
                api.addItem(item.name, item.price.ToString(), item.description);
                items = await api.getAllItems();
            }
           
        }
        public async void updateItem()
        {
            //Debug.WriteLine(selectedItem.ID);
           
            Item item = SelectedItem;
            item.name = itemName;
            item.price = int.Parse(itemPrice);
            item.description = itemDescription;
            itemName = "";
            itemPrice = "";
            itemDescription = "";
            api.updateItem(item);
            items = await api.getAllItems();
        }
        public async void deleteItem()
        {
            string id = "1";
            _responseMsg = await api.deleteItem(id);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            Debug.WriteLine(name);
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
            handler(this, new PropertyChangedEventArgs("ErrorText"));
        }
    }
}
