using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PlexureAPITest
{
    [TestFixture]
    public class Test
    {
        Service service;

        [OneTimeSetUp]
        public void Setup()
        {
            service = new Service();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            if (service != null)
            {
                service.Dispose();
                service = null;
            }
        }

        [Category("Login")]
        [Test]
        public void TEST_001_Login_With_Valid_User()
        {
            var response = service.Login("Tester", "Plexure123");

            response.Expect(HttpStatusCode.OK);
           
        }

        [Category("Points")]
        [Test]
        public void TEST_002_Get_Points_For_Logged_In_User()
        {
            var points = service.GetPoints();

            Console.WriteLine("points : "+points.Entity.Value);
            points.Expect(HttpStatusCode.Accepted);

        }

        [Category("Purchase")]
        [Test]
        public void TEST_003_Purchase_Product()
        {
            int productId = 1;
            var purchaseResponse = service.Purchase(productId);

            purchaseResponse.Expect(HttpStatusCode.Accepted);
        }

        [Category("Purchase")]
        [Test]
        public void TEST_004_Purchase_Listof_Products()
        {
            List<int> productList = new List<int>();
            
            //Add product Ids to the list
            productList.Add(1);
            productList.Add(2);
            productList.Add(3);

            foreach (int productId in productList)
            {
                Thread.Sleep(2000);
                
                service.Purchase(productId);
                
            }
            
        }

        [Category("Purchase")]
        [Test]
        public void TEST_005_Purchase_Unavailable_Product()
        {

            int productId = 912031;
            var purchaseEntity = service.Purchase(productId);
            purchaseEntity.Expect(HttpStatusCode.BadRequest);

        }

        [Category("Purchase")]
        [Test]
        public void TEST_006_Purchase_Product_with_Invalid_ID()
        {

            int productId = 91202331;
            var purchaseEntity = service.Purchase(productId);
            purchaseEntity.Expect(HttpStatusCode.BadRequest);

        }
        
        [Category("Purchase")]
        [Test]
        public void TEST_007_Purchase_Product_Without_Successful_Login()
        {

            int productId = 1;

            var purchaseEntity = service.Purchase(productId);
            
            purchaseEntity.Expect(HttpStatusCode.Unauthorized);
        }

        [Category("Login")]
        [Test]
        public void TEST_008_Login_With_InValid_UserData()
        {
            string username = "Arosha";
            string password = "Arosha123";

            var response = service.Login(username, password);

            response.Expect(HttpStatusCode.Unauthorized);
        }

        [Category("Login")]
        [Test]
        public void TEST_009_Login_With_Missing_Username_Password()
        {
            string username = "";
            string password = "";
            
            var response = service.Login(username, password);

            response.Expect(HttpStatusCode.BadRequest);

        }

        [Category("Points")]
        [Test]
        public void TEST_010_Get_Points_Without_User_Logged_in()
        {
            var points = service.GetPoints();
            
            points.Expect(HttpStatusCode.Unauthorized);

        }
    }
}
