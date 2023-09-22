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
using System.Windows.Controls.Primitives;


namespace materials_management.Models
{
    public class DatabaseModel: ObservableObject
    {
        /* 윈도우 */
        //private string DbSource = "DESKTOP-E2KPEDB\\SQLEXPRESS"; // DB address
        //private string DbName = "sampledb"; // DB database name
        //private string DbUser = "sa";    // DB user name
        //private string DbPassword = "q1234"; // DB pw

        /* 맥 */
        private string DbSource = "LCSC16V986\\SQLEXPRESS"; // DB address
        private string DbName = "materialdb"; // DB database name
        private string DbUser = "sa";    // DB user name
        private string DbPassword = "123123"; // DB pw


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


                    MaterialGroup materialGroupInfo = new MaterialGroup
                    {
                        MaterialGroupCode = materialGroupCode,
                        MaterialGroupName = materialGroupName,
                        MaterialGroupSelection = useFlag,
                        MaterialGroupCreateDate = crtDt,
                        MaterialGroupUpdateDate = udtDt
                    };

                    materialGroupList.Add(materialGroupInfo);
                }
            }
            return materialGroupList;
        }


        // 코드명만 가져오도록 수정
        // -> property로 mainview에서 갖고오도록 수정
        public ObservableCollection<string> GetCodeNames()
        {
            string sql = "SELECT DISTINCT CODE_NAME FROM com_code";

            ObservableCollection<string> codeNames = new ObservableCollection<string>();
            codeNames.Add("ALL");

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
                    string crtDt = Convert.ToDateTime(reader["CRT_DT"]).ToString("yyyy-mm-dd");
                    string udtDt = Convert.ToDateTime(reader["UDT_DT"]).ToString("yyyy-mm-dd");

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
                }
            }
            return materialInfoList;
        }


        /* 데이터 조회 함수 */
        public ObservableCollection<MaterialInfoModel> SearchMaterialInfo(string searchMaterialCode, string searchMaterialName, string searchGroupItem, string searchUseItem)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT MI.MATERIAL_CODE, MI.MATERIAL_NAME, CC.CODE_NAME, CCI.USE_FLAG, MI.CRT_DT, MI.UDT_DT ");
            sqlBuilder.Append("FROM materials_info AS MI ");
            sqlBuilder.Append("INNER JOIN com_code_item AS CCI ON MI.MATERIAL_CODE = CCI.CODE_ITEM_ID ");
            sqlBuilder.Append("INNER JOIN COM_CODE AS CC ON CC.CODE_ID = CCI.CODE_ID ");

            bool hasWhereClause = false; // WHERE 절 추가 여부를 추적

            if (!string.IsNullOrEmpty(searchMaterialCode))
            {
                sqlBuilder.Append(hasWhereClause ? " OR " : " WHERE ");
                sqlBuilder.Append($"MI.MATERIAL_CODE LIKE '%{searchMaterialCode}%' ");
                hasWhereClause = true;
            }

            if (!string.IsNullOrEmpty(searchMaterialName))
            {
                sqlBuilder.Append(hasWhereClause ? " OR " : " WHERE ");
                sqlBuilder.Append($"MI.MATERIAL_NAME LIKE '%{searchMaterialName}%' ");
                hasWhereClause = true;
            }

            if (!string.IsNullOrEmpty(searchGroupItem))
            {
                sqlBuilder.Append(hasWhereClause ? " OR " : " WHERE ");
                sqlBuilder.Append($"CC.CODE_NAME = '{searchGroupItem}' ");
                hasWhereClause = true;
            }

            if (!string.IsNullOrEmpty(searchUseItem))
            {
                sqlBuilder.Append(hasWhereClause ? " OR " : " WHERE ");
                sqlBuilder.Append($"CCI.USE_FLAG = '{searchUseItem}' ");
                hasWhereClause = true;
            }

            string sql = sqlBuilder.ToString();

            ObservableCollection<MaterialInfoModel> materialInfoList = new ObservableCollection<MaterialInfoModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(sql, conn);

                try
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string materialCode = reader["MATERIAL_CODE"].ToString();
                        string materialName = reader["MATERIAL_NAME"].ToString();
                        string materialGroup = reader["CODE_NAME"].ToString(); 
                        string useFlag = reader["USE_FLAG"].ToString();
                        string crtDt = Convert.ToDateTime(reader["CRT_DT"]).ToString("yyyy-mm-dd");
                        string udtDt = Convert.ToDateTime(reader["UDT_DT"]).ToString("yyyy-mm-dd");


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
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("오류 발생: " + ex.Message);
                }
            }

            return materialInfoList;
        }


        /* 삭제 함수 */
        public void DeleteMaterialInfo(string selectedMaterialCode)
        {
            string sql = $"DELETE FROM materials_info WHERE MATERIAL_CODE = '{selectedMaterialCode}'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
