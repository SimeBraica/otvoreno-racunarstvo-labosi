﻿namespace API.Models.ResponseWrapper {
    public class ResponseWrapper<T> {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
