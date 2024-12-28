using Microsoft.VisualBasic;

namespace API.Models.ResponseWrapper {
    public static class ResponseWrapperMethods<T> {
        public static ResponseWrapper<T> ReturnSuccess200(T data) {
            return new ResponseWrapper<T> {
                StatusCode = 200,
                Message = "Action was succesfull",
                Data = data
            };
        }
        public static ResponseWrapper<T> ReturnNotFound404() {
            return new ResponseWrapper<T> {
                StatusCode = 404,
                Message = "Action was not succesfull, resource does not exist",
                Data = default
            };
        }
        public static ResponseWrapper<T> ReturnServerError500() {
            return new ResponseWrapper<T> {
                StatusCode = 500,
                Message = "Action was not succesfull, internal server error",
                Data = default
            };
        }
        public static ResponseWrapper<T> BadRequest400() {
            return new ResponseWrapper<T> {
                StatusCode = 400,
                Message = "Action was not succesfull, search term invalid",
                Data = default
            };
        }
        public static ResponseWrapper<T> Created201() {
            return new ResponseWrapper<T> {
                StatusCode = 201,
                Message = "Action was succesfull, resource created",
                Data = default
            };
        }
    }
}
