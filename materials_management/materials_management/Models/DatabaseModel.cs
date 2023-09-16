using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static materials_management.MainWindow;
using System.Windows;
using Microsoft.SqlServer.Server;
using System.Xml.Linq;


using static materials_management.Models.MaterialInfoModel;


namespace materials_management.Models
{
    public class DatabaseModel: ObservableObject
    {

        // 자재그룹명 데이터만 가져오기 -> combobox
        //public class ComboList
        //{
        //    public List<string> Combo { get; set; }
        //}


        // db 클래스
        public class DbConnector
        {
            private string DbSource = "DESKTOP-E2KPEDB\\SQLEXPRESS"; // DB address
            private string DbName = "sampledb"; // DB database name
            private string DbUser = "sa";    // DB user name
            private string DbPassword = "q1234"; // DB pw

            private string connectionString;


            /* db 연결 메서드 */
            public void Connect()
            {
                connectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", DbSource, DbName, DbUser, DbPassword);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        MessageBox.Show("Connection successful.");
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show("Error connecting to database: " + ex.Message);
                    }

                    connection.Close();
                }
            }

            /* db에서 자재그룹 코드명 데이터 가져오기 */
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


            /* 자재정보 전체 출력 */
            /* db에서 자재정보 테이블 가져오기 */
            public ObservableCollection<MaterialInfoModel> GetMaterialInfoFromDatabase()
            {
                string sql = "SELECT * FROM materials_info";

                ObservableCollection<MaterialInfoModel> materialInfoList = new ObservableCollection<MaterialInfoModel>();


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
                        DateTime udtDt = Convert.ToDateTime(reader["UDT_DT"]);


                        // Model -> MaterialInfo 객체 생성
                        MaterialInfoModel materialInfo = new MaterialInfoModel
                        {
                            MaterialCode = materialCode,
                            MaterialName = materialName,
                            MaterialGroupName = materialGroup,
                            MaterialUseSelection = useFlag,
                            MaterialCreateDate = crtDt,
                            MaterialUpdateDate = udtDt
                        };

                        // 생성한 MaterialInfo 객체를 ObservableCollection에 추가
                        materialInfoList.Add(materialInfo);
                    }
                }
                return materialInfoList;
            }
        }
    }
}
