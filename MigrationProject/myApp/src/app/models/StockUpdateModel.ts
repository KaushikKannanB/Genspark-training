export class StockUpdateModel {
  constructor(
    public ProductName?:string,
    public AddOrReduce?:string,
    public AddOrReduceBy?:number
  ) { }
}
