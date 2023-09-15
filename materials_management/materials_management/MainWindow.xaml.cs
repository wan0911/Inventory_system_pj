using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Windows.Themes;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using System.Collections;
using static materials_management.MainWindow;

namespace materials_management
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // DB 연결
            DbConnector dbConnector = new DbConnector();
            dbConnector.Connect();


            // serach: 자재그룹 db 데이터 연결
            ComboList comboList = new ComboList();
            comboList.Combo = dbConnector.GetCodeNames(); // codeNames를 Combo 속성에 대입
            MGSearchCombo.DataContext = comboList;


            // 그리드뷰에 자재정보 연결
            // 데이터베이스에서 자재 정보 가져오기
            ObservableCollection<MaterialIfno> materialInfoList = dbConnector.GetMaterialInfoFromDatabase();
            // 그리드뷰에 데이터 바인딩
            materialDataGrid.ItemsSource = materialInfoList;
        }


        // 자재그룹명 데이터만 가져오기 -> combobox
        public class ComboList
        {
             public List<string> Combo { get; set; }
        }


        // 자재정보 데이터
        public class MaterialIfno
        {
            public string MaterialCode { get; set; }
            public string MaterialName { get; set; }
            public string MaterialGroup { get; set; }

            public string MaterialUseFlag { get; set; }
            public DateTime MaterialCrtDt { get; set; }

        }



        // db 클래스
        public class DbConnector
        {
            string DbSource = "DESKTOP-E2KPEDB\\SQLEXPRESS"; // DB address
            string DbName = "sampledb"; // DB database name
            string DbUser = "sa";    // DB user name
            string DbPassword = "q1234"; // DB pw



            public void Connect()
            {
                string connectionString = $"Data Source={DbSource};" + $"Initial Catalog={DbName};" +  $"User ID={DbUser};" +  $"Password={DbPassword};";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        MessageBox.Show("Connection successful.");
                    }
                    catch (Exception ex) // DB connection failed
                    {
                        MessageBox.Show("Error connecting to database: " + ex.Message);
                    }

                    connection.Close();
                }
            }

            // db에서 자재그룹 코드명 가져오기
            public List<string> GetCodeNames()
            {
                string connectionString = $"Data Source={DbSource};" + $"Initial Catalog={DbName};" + $"User ID={DbUser};" + $"Password={DbPassword};";
                string sql = "SELECT CODE_NAME FROM com_code";

                List<string> codeNames = new List<string>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string codeName = reader["CODE_NAME"].ToString();
                        codeNames.Add(codeName);
                    }
                }
                return codeNames;
            }

            // db에서 자재정보 가져오기
            public ObservableCollection<MaterialIfno> GetMaterialInfoFromDatabase()
            {
                string connectionString = $"Data Source={DbSource};" + $"Initial Catalog={DbName};" + $"User ID={DbUser};" + $"Password={DbPassword};";
                string sql = "SELECT * FROM materials_info";


                ObservableCollection<MaterialIfno> materialInfoList = new ObservableCollection<MaterialIfno>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string materialCode = reader["MATERIAL_CODE"].ToString();
                        string materialName = reader["MATERIAL_NAME"].ToString();
                        string materialGroup = reader["MATERIAL_GROUP"].ToString();
                        string useFlag = reader["USE_FLAG"].ToString();
                        DateTime crtDt = Convert.ToDateTime(reader["CRT_DT"]);


                        // MaterialInfo 객체 생성
                        MaterialIfno materialInfo = new MaterialIfno
                        {
                            MaterialCode = materialCode, 
                            MaterialName = materialName,
                            MaterialGroup = materialGroup,
                            MaterialUseFlag = useFlag,
                            MaterialCrtDt = crtDt
                        };

                        // 생성한 MaterialInfo 객체를 ObservableCollection에 추가
                        MessageBox.Show($"{materialInfo[0]}");
                        materialInfoList.Add(materialInfo);

                    }
                }

                return materialInfoList;
            }
        }




        private void MaterialGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            //var subView = new Views.MainView()
            //{
            //    DataContxt = new ViewModels.MainViewModel();
            //};
        }


    }
}
