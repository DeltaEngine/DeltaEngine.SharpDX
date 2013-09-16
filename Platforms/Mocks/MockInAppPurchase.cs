namespace DeltaEngine.Platforms.Mocks
{
	/// <summary>
	/// Mock in-app purchase used for unit testing.
	/// </summary>
	public class MockInAppPurchase : InAppPurchase
	{
		public override bool RequestProductInformationAsync(int[] productIds)
		{
			var product = new ProductData("testId", "testTitle", "testDescription", "testPrice", true);
			InvokeOnReceivedProductInformation(new[] { product });
			return true;
		}

		public override bool PurchaseProductAsync(string productId)
		{
			InvokeOnTransactionFinished(productId, true);
			return true;
		}
	}
}