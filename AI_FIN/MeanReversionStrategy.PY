import backtrader as bt
import yfinance as yf

#逢高賣 逢低買

class MeanReversionStrategy(bt.Strategy):
    params = (
        ("lookback_period", 20),
        ("entry_z_score", 1.0),
        ("exit_z_score", 0.0),
    )

    def __init__(self):
        self.data_close = self.datas[0].close
        self.mean = bt.indicators.SMA(self.data_close, period=self.params.lookback_period)
        self.stddev = bt.indicators.StdDev(
            self.data_close, period=self.params.lookback_period
        )

    def next(self):
        z_score = (self.data_close[0] - self.mean[0]) / self.stddev[0]
        if not self.position and z_score >= self.params.entry_z_score:
            self.sell(size=10)  # 逢高賣出 10 股
        elif self.position and z_score <= self.params.exit_z_score:
            self.buy(size=10)  # 逢低買進 10 股

    def stop(self):
        print(f"Final Portfolio Value: {self.broker.getvalue():,.2f} USD")


# 初始化 cerebro
cerebro = bt.Cerebro()

# 設定資料來源
data = bt.feeds.PandasData(dataname=yf.download("AAPL", start="2022-01-01",end="2023-03-12"))
cerebro.adddata(data)

# 設定策略
cerebro.addstrategy(
    MeanReversionStrategy,
    lookback_period=20,
    entry_z_score=1.0,
    exit_z_score=0.0,
)

# 設定初始資金為 10,000 美元
cerebro.broker.setcash(10000.0)

# 設定每筆交易固定為 10 股
cerebro.addsizer(bt.sizers.FixedSize, stake=10)

# 設定手續費為 0.1%
cerebro.broker.setcommission(commission=0.001)

# 開始回測
cerebro.run()
