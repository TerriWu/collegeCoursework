import backtrader as bt
import yfinance as yf

#RSI

class RSIStrategy(bt.Strategy):
    params = (
        ("rsi_periods", 14),
        ("rsi_upper", 85),
        ("rsi_lower", 25),
        ("printlog", False),
    )

    def __init__(self):
        self.rsi = bt.indicators.RSI(
            period=self.params.rsi_periods,
            upperband=self.params.rsi_upper,
            lowerband=self.params.rsi_lower,
        )

    def next(self):
        if self.rsi > self.params.rsi_upper:
            self.sell()
        elif self.rsi < self.params.rsi_lower:
            self.buy()

        if self.params.printlog:
            print(f"RSI: {self.rsi[0]}")

# 初始化 cerebro
cerebro = bt.Cerebro()

# 設定資料來源
data = bt.feeds.PandasData(dataname=yf.download("0056.TW", start="2018-01-01",end="2023-03-12"))
cerebro.adddata(data)

# 設定策略
cerebro.addstrategy(RSIStrategy)

# 設定初始資金為 10,000 美元
cerebro.broker.setcash(10000.0)

# 設定每筆交易固定為 10 股
cerebro.addsizer(bt.sizers.FixedSize, stake=10)

# 設定手續費為 0.1%
cerebro.broker.setcommission(commission=0.001)

# 開始回測
cerebro.run()

# 印出最終資金餘額
print(f"Final Portfolio Value: {cerebro.broker.getvalue():,.2f} USD")
