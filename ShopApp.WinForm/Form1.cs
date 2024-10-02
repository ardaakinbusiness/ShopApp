using ShopApp.BLL.Abstract;
using ShopApp.DAL.Concrete.EfCore;
using ShopApp.Entity;

namespace ShopApp.WinForm
{
    public partial class Form1 : Form
    {
        EfCoreProductDal _productDal = new EfCoreProductDal();
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_getir_Click(object sender, EventArgs e)
        {
            var liste = _productDal.GetAll();

            foreach (Product item in liste)
            {
                listBox1.Items.Add(item.Name);
            }
        }
    }
}