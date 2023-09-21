using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace materials_management.DataBase
{
    public abstract class DatabaseService : IDatabaseService
    {
        private string _connectionString;
        public string ConnectionString => _connectionString;


        protected DbConnection Connection { get; set; }

        protected DbCommand Command { get; set; }


        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }


        public virtual async Task<ObservableCollection<T>> GetDatasAsync<T>(string query) where T : class
        {
            if (Connection == null || Command == null || string.IsNullOrEmpty(query))
            {
                return null;
            }
            //컨넥션 열기
            await Connection.OpenAsync();
            //쿼리 입력
            Command.CommandText = query;
            Command.Connection = Connection;

            var returnDatas = new ObservableCollection<T>();

            using var reader = await Command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = (IDataRecord)reader;
                var model = Activator.CreateInstance(typeof(T));
                returnDatas.Add(model as T);


                //이 아래 부분은 프로퍼티 한개씩 하드코딩 하지 않고, 값을 입력하기 위해서 사용하는 부분입니다.
                //모델에서 프로퍼티 추출
                var propertys = model.GetType().GetProperties();
                //프로퍼티 중 HasErrors라는 이름의 프로퍼티 빼고 나머지 데이터 입력
                foreach (var prop in propertys.Where(p => p.Name != "HasErrors"))
                {
                    try
                    {
                        var value = row[prop.Name];
                        if (value is DBNull == false)
                        {
                            prop.SetValue(model, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }


            await Connection.CloseAsync();


            return returnDatas;
        }
    }
}
