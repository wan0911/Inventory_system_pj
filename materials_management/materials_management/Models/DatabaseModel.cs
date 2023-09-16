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
        private string DbSource = "DESKTOP-E2KPEDB\\SQLEXPRESS"; // DB address
        private string DbName = "sampledb"; // DB database name
        private string DbUser = "sa";    // DB user name
        private string DbPassword = "q1234"; // DB pw

        private string connectionString;

        /* db 연결 */
        // 싱글톤 패턴 적용 필요
        private static DatabaseModel conn;

        private DatabaseModel() { }

        public static DatabaseModel Getins()
        {
            if (conn == null)
            {
                conn = new DatabaseModel();
            }
            return conn;
        }

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



        /* 자재그룹 데이터 가져오기 */
        public ObservableCollection<MaterialGroup> GetMaterialGroupFromDatabase()
        {
            string sql = "SELECT CODE_NAME FROM com_code";

            ObservableCollection<MaterialGroup> materialGroupList = new ObservableCollection<MaterialGroup>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string materialGroupCode = reader["CODE_ID"].ToString();
                    string materialGroupName = reader["CODE_NAME"].ToString();
                    string materialGroupSelection = reader["USE_FLAG"].ToString();
                    string useFlag = reader["USE_FLAG"].ToString();
                    DateTime crtDt = Convert.ToDateTime(reader["CRT_DT"]);
                    DateTime udtDt = Convert.ToDateTime(reader["UDT_DT"]);


                    // Model -> MaterialInfo 객체 생성
                    MaterialGroup materialGroupInfo = new MaterialGroup
                    {
                        MaterialGroupCode = materialGroupCode,
                        MaterialGroupName = materialGroupName,
                        MaterialGroupSelection = useFlag,
                        MaterialGroupCreateDate = crtDt,
                        MaterialGroupUpdateDate = udtDt
                    };

                    // 생성한 MaterialInfo 객체를 ObservableCollection에 추가
                    materialGroupList.Add(materialGroupInfo);
                }
            }
            return materialGroupList;
        }


        // 코드명만 가져오도록 수정
        public ObservableCollection<string> GetCodeNames()
        {
            string sql = "SELECT CODE_NAME FROM com_code";

            ObservableCollection<string> codeNames = new ObservableCollection<string>();

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


        /* 데이터 조회 함수 */

        public ObservableCollection<MaterialInfoModel> SearchMaterialInfo(string searchText1, string searchText2)
        {
            string sql = "SELECT MATERIAL_CODE, MATERIAL_NAME, MATERIAL_GROUP, USE_FLAG, CRT_DT, UDT_DT FROM materials_info WHERE MATERIAL_CODE = @SearchText1 OR MATERIAL_NAME = @SearchText2";


            ObservableCollection<MaterialInfoModel> materialInfoList = new ObservableCollection<MaterialInfoModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@SearchText1", searchText1);
                command.Parameters.AddWithValue("@SearchText2", searchText2);
                MessageBox.Show($"{command}");

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string materialCode = reader["MATERIAL_CODE"].ToString();
                        string materialName = reader["MATERIAL_NAME"].ToString();
                        string materialGroup = reader["MATERIAL_GROUP"].ToString();
                        string useFlag = reader["USE_FLAG"].ToString();
                        DateTime crtDt = Convert.ToDateTime(reader["CRT_DT"]);
                        DateTime udtDt = Convert.ToDateTime(reader["UDT_DT"]);

                        MaterialInfoModel materialInfo = new MaterialInfoModel
                        {
                            MaterialCode = materialCode,
                            MaterialName = materialName,
                            MaterialGroupName = materialGroup,
                            MaterialUseSelection = useFlag,
                            MaterialCreateDate = crtDt,
                            MaterialUpdateDate = udtDt
                        };

                        materialInfoList.Add(materialInfo);
                        int a = 0;
                    }
                }
                catch (Exception ex)
                {
                    // 오류 처리 코드 추가
                    Console.WriteLine("오류 발생: " + ex.Message);
                }
            }
            return materialInfoList;
        }


    }
}
