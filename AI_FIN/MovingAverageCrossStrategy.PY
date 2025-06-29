import backtrader as bt
import yfinance as yf


class MovingAverageCrossStrategy(bt.Strategy):
    params = (("fast_ma", 10), ("slow_ma", 30))

    def __init__(self):
        self.data_close = self.datas[0].close
        self.fast_ma = bt.indicators.SMA(self.data_close, period=self.params.fast_ma)
        self.slow_ma = bt.indicators.SMA(self.data_close, period=self.params.slow_ma)
        self.crossover = bt.indicators.CrossOver(self.fast_ma, self.slow_ma)

    def next(self):
        if not self.position and self.crossover > 0:
            self.buy(size=10)  # 買進 10 股
        elif self.position and self.crossover < 0:
            self.sell(size=10)  # 賣出 10 股

    def stop(self):
        print(f"Final Portfolio Value: {self.broker.getvalue():,.2f} USD")


# 初始化 cerebro
cerebro = bt.Cerebro()

# 設定資料來源
data = bt.feeds.PandasData(dataname=yf.download("0050.TW", start="2021-01-01",end="2021-12-31"))
cerebro.adddata(data)

# 設定策略
cerebro.addstrategy(MovingAverageCrossStrategy, fast_ma=10, slow_ma=30)

# 設定初始資金為 10,000 美元
cerebro.broker.setcash(10000.0)

# 設定每筆交易固定為 10 股
cerebro.addsizer(bt.sizers.FixedSize, stake=10)

# 設定手續費為 0.1%
cerebro.broker.setcommission(commission=0.001)

# 開始回測
cerebro.run()
