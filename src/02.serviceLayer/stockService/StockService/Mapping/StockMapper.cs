using sharedMethods.Mapping;
using stockRepository.Entities;
using stockService.StockService.Models;

namespace stockService.StockService.Mapping;

public static class StockMapper
{
    public static Stock? Map(this StockDto stockDto)
    {
        var destination = Mapper.Map<StockDto, Stock>(stockDto);
        return destination;
    }
    
   public static List<Stock>? Map(this List<StockDto> stockDto)
    {
        var destination = Mapper.MapList<StockDto,Stock>(stockDto);
        return destination!.ToList();
    }
    
    public static StockDto? Map(this Stock stock)
    {
        var destination = Mapper.Map<Stock, StockDto>(stock);
        return destination;
    }
    
    public static List<StockDto>? Map(this List<Stock> stocks)
    {
        var destination = Mapper.MapList<Stock,StockDto>(stocks);
        return destination!.ToList();
    }
}
