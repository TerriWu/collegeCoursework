import backtrader as bt
import yfinance as yf

#開盤

class OpenCloseStrategy(bt.Strategy):
    def __init__(self):
        self.data_open = self.datas[0].open
        self.data_close = self.datas[0].close

    def next(self):
        if not self.position:
            self.buy(size=10)  # 買進 10 股
        else:
            self.sell(size=10)  # 賣出 10 股

    def stop(self):
        print(f"Final Portfolio Value: {self.broker.getvalue():,.2f} USD")


# 初始化 cerebro
cerebro = bt.Cerebro()

# 設定資料來源
data = bt.feeds.PandasData(dataname=yf.download("0050.TW", start="2018-01-01",end="2023-03-12"))
cerebro.adddata(data)

# 設定策略
cerebro.addstrategy(OpenCloseStrategy)

# 設定初始資金為 10,000 美元
cerebro.broker.setcash(10000.0)

# 設定每筆交易固定為 10 股
cerebro.addsizer(bt.sizers.FixedSize, stake=10)

# 設定手續費為 0.1%
cerebro.broker.setcommission(commission=0.001)

# 開始回測
cerebro.run()
