using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace materials_management.DataBase
{
    public interface IDatabaseService
    {

        string ConnectionString { get; }
        /// <summary>
        /// GetDatasAsync
        /// </summary>
        /// <remarks>
        /// query를 실행해서 IList&lt;<typeparamref name="T"/>&gt;를 반환한다.
        /// </remarks>
        Task<ObservableCollection<T>> GetDatasAsync<T>(string query) where T : class;
    }
}
