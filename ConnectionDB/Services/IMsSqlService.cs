using ConnectionDB.Model;

namespace ConnectionDB.Services
{
    public interface IMsSqlService
    {
        ConnDTO ViewData(ConnDTO sqlServerDTO);
        ConnDTO IUDData(ConnDTO sqlServerDTO);
    }
    
}
