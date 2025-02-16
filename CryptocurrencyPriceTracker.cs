package main

import (
	"encoding/json"
	"fmt"
	"net/http"
	"time"
)

// API URL for fetching cryptocurrency prices
const apiURL = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,litecoin&vs_currencies=usd"

// Struct for mapping API response
type CryptoPrices struct {
	Bitcoin  struct{ USD float64 `json:"usd"` }  `json:"bitcoin"`
	Ethereum struct{ USD float64 `json:"usd"` }  `json:"ethereum"`
	Litecoin struct{ USD float64 `json:"usd"` }  `json:"litecoin"`
}

// fetchPrices gets cryptocurrency prices from the API
func fetchPrices() (CryptoPrices, error) {
	resp, err := http.Get(apiURL)
	if err != nil {
		return CryptoPrices{}, err
	}
	defer resp.Body.Close()

	var prices CryptoPrices
	err = json.NewDecoder(resp.Body).Decode(&prices)
	if err != nil {
		return CryptoPrices{}, err
	}

	return prices, nil
}

func main() {
	fmt.Println("🚀 Cryptocurrency Price Tracker 🚀")

	for {
		// Fetch live prices
		prices, err := fetchPrices()
		if err != nil {
			fmt.Println("Error fetching prices:", err)
			return
		}

		// Display Prices
		fmt.Printf("\n📈 Bitcoin (BTC): $%.2f\n", prices.Bitcoin.USD)
		fmt.Printf("📉 Ethereum (ETH): $%.2f\n", prices.Ethereum.USD)
		fmt.Printf("🔹 Litecoin (LTC): $%.2f\n", prices.Litecoin.USD)

		// Wait before fetching again
		fmt.Println("\n🔄 Refreshing in 10 seconds...")
		time.Sleep(10 * time.Second)
	}
}
