using DeltaEngine.Platforms.Mocks;
using NUnit.Framework;

namespace DeltaEngine.Platforms.Tests
{
	class InAppPurchaseTests
	{
		[Test]
		public void RequestProductInformtaion()
		{
			var inAppPurchase = new MockInAppPurchase();
			inAppPurchase.OnReceivedProductInformation += ReceivedProductInformation;
			Assert.IsTrue(inAppPurchase.RequestProductInformationAsync(new[] { 0 }));
		}

		private static void ReceivedProductInformation(ProductData[] products)
		{
			Assert.AreEqual("testId", products[0].Id);
			Assert.AreEqual("testTitle", products[0].Title);
			Assert.AreEqual("testDescription", products[0].Description);
			Assert.AreEqual("testPrice", products[0].Price);
			Assert.IsTrue(products[0].IsValid);
		}

		[Test]
		public void PurchaseProductSuccessfully()
		{
			var inAppPurchase = new MockInAppPurchase();
			inAppPurchase.OnTransactionFinished += PurchaseProduct;
			Assert.IsTrue(inAppPurchase.PurchaseProductAsync("testId"));
		}

		private static void PurchaseProduct(string productId, bool wasSuccessful)
		{
			Assert.AreEqual("testId", productId);
			Assert.IsTrue(wasSuccessful);
		}

		[Test]
		public void PurchaseProductCanceled()
		{
			var inAppPurchase = new MockInAppPurchase();
			inAppPurchase.OnUserCanceled += PurchaseCanceled;
			inAppPurchase.InvokeOnUserCanceled("testId");
		}

		private static void PurchaseCanceled(string productId)
		{
			Assert.AreEqual("testId", productId);
		}
	}
}