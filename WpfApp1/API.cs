using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class API
    {
        static HttpClient client = new HttpClient();
        public User loginUser(string nick, string password)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["nick"] = nick;
                data["password"] = password;
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/userLogin.php", "POST", data);
                return JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(response));
            }
        }
        public async Task<User> GetUser(string ID)
        {
            List<User> user = new List<User>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/getUsers.php?ID=" + ID);
            if (response.IsSuccessStatusCode)
            {
                user = JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());
            }
            return user[0];
        }
        public async Task<List<User>> GetAllUsers()
        {
            List<User> user = new List<User>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/getUsers.php");
            if (response.IsSuccessStatusCode)
            {
                user = JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());
            }
            return user;
        }
        public async Task<List<Item>> getItem(string ID)
        {
            List<Item> Items = new List<Item>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/getItems.php?ID=" + ID);
            if (response.IsSuccessStatusCode)
            {
                Items = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());
            }
            return Items;
        }
        public async Task<List<Item>> getAllItems()
        {
            List<Item> items = new List<Item>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/getItems.php");
            if (response.IsSuccessStatusCode)
            {
                items = JsonConvert.DeserializeObject<List<Item>>(await response.Content.ReadAsStringAsync());
            }
            return items;
        }
        public async Task<List<Order>> getUserOrders(string ID)
        {
            List<Order> orders = new List<Order>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/getOrders.php?ID=" + ID);
            if (response.IsSuccessStatusCode)
            {
                orders = JsonConvert.DeserializeObject<List<Order>>(await response.Content.ReadAsStringAsync());
            }
            return orders;
        }
        public async Task<List<Order>> getAllOrders()
        {
            List<Order> orders = new List<Order>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/getOrders.php");
            if (response.IsSuccessStatusCode)
            {
                orders = JsonConvert.DeserializeObject<List<Order>>(await response.Content.ReadAsStringAsync());
            }
            return orders;
        }
        public string updateUser(User user,string password)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["ID"] = user.ID;
                data["nick"] = user.nick;
                data["password"] = password;
                data["email"] = user.email;
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/updateUser.php", "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }
        public string updateItem(Item item)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["ID"] = item.ID;
                data["name"] = item.name;
                data["price"] = item.price.ToString();
                data["description"] = item.description;
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/updateItem.php ", "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }
        public string updateOrder(Order order)
        {
            Debug.WriteLine(order.ID);
            List<string> itemsIDs = new List<string>();
            foreach (var item in order.items)
            {
                itemsIDs.Add(item.ID);
            }
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["del"] = order.ID;
                data["items"] = JsonConvert.SerializeObject(itemsIDs);
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/updateOrder.php ", "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }
        public string addUser(string nickname,string password,string email)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["nick"] = nickname;
                data["password"] = password;
                data["email"] = email;
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/addUser.php", "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }
        public string addOrder(string userID,List<Item> items)
        {
            List<string> itemsIDs = new List<string>();
            foreach (var item in items)
            {
                itemsIDs.Add(item.ID);
            }
            using (var wb = new WebClient())
            {
                DateTime date = DateTime.Now;
                var data = new NameValueCollection();
                data["userID"] = userID;
                data["date"] = date.ToLongDateString() + " " + date.ToLongTimeString();
                data["items"] = JsonConvert.SerializeObject(itemsIDs);
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/addOrder.php", "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }
        public string addItem(string name, string price, string description)
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["name"] = name;
                data["price"] = price;
                data["description"] = description;
                var response = wb.UploadValues("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/addItem.php", "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }
        public async Task<string> deleteUser(string ID)
        {
            string res = " ";
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/deleteUser.php?ID=" + ID);
            if (response.IsSuccessStatusCode)
            {
                res = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
            return res;
        }
        public async Task<string> deleteOrder(string ID)
        {
            string res = " ";
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/deleteOrder.php?del=" + ID);
            if (response.IsSuccessStatusCode)
            {
                res = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
            return res;
        }
        public async Task<string> deleteItem(string ID)
        {
            string res = " ";
            List<User> user = new List<User>();
            HttpResponseMessage response = await client.GetAsync("https://student.sps-prosek.cz/~mikesma15/Database/objednavky/deteleItem.php?del=" + ID);
            if (response.IsSuccessStatusCode)
            {
                res = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
            }
            return res;
        }

    }
}
