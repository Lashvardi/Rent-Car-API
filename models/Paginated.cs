﻿namespace RentCar.models;

// This class is used for pagination
public class PaginatedData<T>
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public List<T> Data { get; set; }
}
