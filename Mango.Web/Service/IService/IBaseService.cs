using Mango.Web.Models;

namespace Mango.Web.Service.IService;

public interface IBaseService
{
    // Fr means for requestdto
    Task<ResponseDto?> SendAsyncFr(RequestDto requestDto);

}