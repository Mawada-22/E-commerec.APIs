using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared;

public class PictureUrlResolver : IValueResolver<Product, ProductDto, string>
{
    private readonly IConfiguration _configuration;

    public PictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(
        Product source,
        ProductDto destination,
        string destMember,
        ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source.PictureUrl))
            return string.Empty;

        var baseUrl = _configuration["BaseUrl"];

        return string.IsNullOrEmpty(baseUrl)
            ? source.PictureUrl
            : $"{baseUrl}{source.PictureUrl}";
    }
}
