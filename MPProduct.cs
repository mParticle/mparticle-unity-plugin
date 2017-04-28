using System.Collections.Generic;

public class MPProduct : Dictionary<string, string> {
	private const string nameKey = "ProductName";
	private const string skuKey = "ProductSKU";
	private const string affiliationKey = "TransactionAffiliation";
	private const string unitpriceKey = "ProductUnitPrice";
	private const string quantityKey = "ProductQuantity";
	private const string revenueKey = "RevenueAmount";
	private const string taxKey = "TaxAmount";
	private const string shippingKey = "ShippingAmount";
	private const string categoryKey = "ProductCategory";
	private const string currencyKey = "CurrencyCode";
	private const string transactionIdKey = "TransactionID";

	public MPProduct(string productName, string productSku)
	{
		this.ProductName = productName;
		this.ProductSku = productSku;
	}

	public string ProductName
	{
		get 
		{
			string productName = "";
			this.TryGetValue(nameKey, out productName);
			return productName;
		} 
		private set {
			this[nameKey] = value;
		}
	}

	public string ProductSku
	{
		get 
		{
			string productSku = "";
			this.TryGetValue(skuKey, out productSku);
			return productSku;
		} 
		private set {
			this[skuKey] = value;
		}
	}

	public double UnitPrice
	{
		get 
		{
			string price = "0";
			this.TryGetValue(unitpriceKey, out price);
			return double.Parse(price);
		} 
		set {
			this[unitpriceKey] = value.ToString();
		}
	}

	public int Quantity {
		get 
		{
			string quantity = "0";
			this.TryGetValue(unitpriceKey, out quantity);
			return int.Parse(quantity);
		}
		set {
			this[quantityKey] = value.ToString ();
		}
	}

	public string ProductCategory
	{
		get 
		{
			string category = "";
			this.TryGetValue(categoryKey, out category);
			return category;
		} 
		set {
			this[categoryKey] = value;
		}
	}

	public double TotalRevenue
	{
		get 
		{
			string totalRevenue = "0";
			this.TryGetValue(revenueKey, out totalRevenue);
			return double.Parse(totalRevenue);
		}
		set
		{
			this[revenueKey] = value.ToString();
		}

	}

	public double TaxAmount
	{
		get 
		{
			string taxAmount = "0";
			this.TryGetValue(taxKey, out taxAmount);
			return double.Parse(taxAmount);
		}
		set
		{
			this[taxKey] = value.ToString();
		}
	}

	public double ShippingAmount
	{
		get 
		{
			string shippingAmount = "0";
			this.TryGetValue(shippingKey, out shippingAmount);
			return int.Parse(shippingAmount);
		}
		set
		{
			this[shippingKey] = value.ToString();
		}
	}

	public string CurrencyCode
	{
		get 
		{
			string currencyCode = "";
			this.TryGetValue(currencyKey, out currencyCode);
			return currencyCode;
		}
		set
		{
			this[currencyKey] = value;
		}

	}

	string Affiliation
	{
		get 
		{
			string affiliation = "";
			this.TryGetValue(affiliationKey, out affiliation);
			return affiliation;
		}
		set
		{
			this[affiliationKey] = value;
		}

	}

	public string TransactionId
	{
		get 
		{
			string transactionId = "";
			this.TryGetValue(transactionIdKey, out transactionId);
			return transactionId;
		}
		set
		{
			this[transactionIdKey] = value;
		}

	}
}