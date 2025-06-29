import backtrader as bt
import yfinance as yf

class GoldenDeathCrossStrategy(bt.Strategy):
    
    params = (('fast', 50), ('slow', 200))
    
    def __init__(self):
        self.fastma = bt.indicators.SMA(self.data.close, period=self.params.fast)
        self.slowma = bt.indicators.SMA(self.data.close, period=self.params.slow)
        self.crossover = bt.indicators.CrossOver(self.fastma, self.slowma)
    
    def next(self):
        if not self.position:
            if self.crossover > 0:
                self.buy()
        elif self.crossover < 0:
            self.close()

# 载入数据
data = bt.feeds.PandasData(dataname=yf.download('AAPL', start='2018-01-01', end='2023-03-12'))

# 初始化cerebro
cerebro = bt.Cerebro()

# 添加数据
cerebro.adddata(data)

# 添加策略
cerebro.addstrategy(GoldenDeathCrossStrategy)

# 设置初始资金
cerebro.broker.setcash(100000)

# 设置每次交易的股票数量
cerebro.addsizer(bt.sizers.PercentSizer, percents=10)

# 运行回测
cerebro.run()

# 打印回测结果
print(f"最终资产总值: {cerebro.broker.getvalue():.2f} USD")
