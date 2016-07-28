using Foundation;
using System;
using UIKit;
using AppServiceHelpers.Abstractions;
using AppServiceHelpers;
using TraditionalSample.iOS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraditionalSample.iOS
{
    public partial class MyTableView : UITableViewController, IUITableViewDataSource
    {
        IEasyMobileServiceClient client;
        ITableDataStore<ToDo> dataStore;
        IEnumerable<ToDo> items;

        public MyTableView (IntPtr handle) : base (handle)
        {
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            EasyMobileServiceClient.Current.Initialize("http://xamarin-todo-sample.azurewebsites.net");
            EasyMobileServiceClient.Current.RegisterTable<ToDo>();
            await EasyMobileServiceClient.Current.FinalizeSchema();

            dataStore = AppServiceHelpers.EasyMobileServiceClient.Current.Table<ToDo>();
            await dataStore.Sync();
            items = await dataStore.GetItemsAsync();

            TableView.DataSource = this;
            TableView.Delegate = this;
            TableView.ReloadData();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 0;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            var count = dataStore.Count();
            return count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var todo = items.ElementAt(indexPath.Row);
            var cell = tableView.DequeueReusableCell("MyCell") as MyCustomCell;

            cell.Todo = todo;
            return cell;
        }
    }
}