using System.Collections;

namespace OtoKurSkoda.Application.Defaults
{
    public abstract class BaseApplicationService
    {

        protected Result SuccessResult(string messageCode = "", string message = "")
        {
            return new Result(status: true, messageCode, message, ResultStatusCode.Success);
        }
        protected DataResult<T> SuccessDataResult<T>(T resultData, string messageCode = "", string message = "")
        {
            return new DataResult<T>(resultData, status: true, messageCode, message, ResultStatusCode.Success);
        }
        protected DataResult<T> ErrorDataResult<T>(T resultData, string messageCode = "", string message = "")
        {
            return new DataResult<T>(resultData, status: false, messageCode, message, ResultStatusCode.Error);
        }
        protected ListDataResult<T> SuccessListDataResult<T>(T resultData, string messageCode = "", string message = "") where T : ICollection
        {
            return new ListDataResult<T>(resultData, status: true, messageCode, message, ResultStatusCode.Success);
        }

        protected Result ErrorResult(string messageCode = "", string message = "")
        {
            return new Result(status: false, messageCode, message, ResultStatusCode.Error);
        }

        protected Result WarningResult(string messageCode = "", string message = "")
        {
            return new Result(status: true, messageCode, message, ResultStatusCode.Warning);
        }

        protected PagedDataResult<T> SuccessPagedDataResult<T>(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize, string messageCode = "", string message = "")
        {
            var pagedResult = new PagedResult<T>
            {
                Items = items.ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return new PagedDataResult<T>(pagedResult, status: true, messageCode, message, ResultStatusCode.Success);
        }
    }
}

