import pandas as pd
import yfinance as yf
import csv

def risk_vola(stcok_name,stock_thecode):

    # 下載蘋果電腦公司的股市交易數據
    stock = yf.download(stock_thecode, start="2009-01-01", end="2022-12-31")
    # stock = yf.download(stock_thecode, start="2020-01-01", end="2023-12-31")
    # 將數據存成csv檔案
    stock.to_csv('stock.csv')

    # 計算日收益率
    daily_returns = stock['Adj Close'].pct_change()

    # 計算平均日收益率
    avg_daily_return = daily_returns.mean()

    # 計算平均年收益率
    avg_annual_return = avg_daily_return * 252

    # # 計算日波動率
    # daily_volatility = daily_returns.std()

    # # 計算年波動率
    # annual_volatility = daily_volatility * (252 ** 0.5)


    #打開csv
    # file=open('0427_2.csv',mode='w',newline='')
    writer = csv.writer(file)
    # 寫入標題行（可選）
    writer.writerow(['股票名稱','平均日收益率', '平均年收益率'])
    # 寫入數據行
    writer.writerow([stcok_name,avg_daily_return, avg_annual_return])
    # file.close()

    # 將計算後的結果列印在螢幕上
    # print("公司日收益率：", daily_returns.tail())
    print(stcok_name+"公司平均日收益率：", avg_daily_return)
    print(stcok_name+"公司平均年收益率：", avg_annual_return)
    # print("蘋果電腦公司日波動率：", daily_volatility)
    # print("蘋果電腦公司年波動率：", annual_volatility)

# stcok_name=["^TWII"]
# stock_thecode=["2301.TW"]

# stcok_name=["光寶科", "全友", "仁寶", "精英", "佳世達", "宏碁", "英業達", "華碩", "藍天", "倫飛", "昆盈", "技嘉", "微星", "虹光", "廣達", "精元", "研華"
#                 , "友通", "映泰", "輔信", "圓剛", "隴華", "承啟", "新美齊", "麗臺", "普安", "歐格", "神基", "晟銘電", "奇鋐", "威強電", "建碁", "喬鼎"
#                     , "銘異", "緯創", "融程電", "誠研", "華擎", "精聯", "大眾控", "神達", "永崴投控", "事欣科", "和碩", "科嘉-KY", "虹堡", "迎廣", "上福", "凌華"
#                         , "飛捷", "尼得科超眾", "華孚", "宏正", "樺漢", "研揚", "動力-KY", "緯穎", "振樺電", "達方", "勤誠", "偉聯"]

# stock_thecode=["2301.TW", "2305.TW", "2324.TW", "2331.TW", "2352.TW", "2353.TW", "2356.TW", "2357.TW", "2362.TW", "2364.TW", "2365.TW", "2376.TW", "2377.TW"
#                 , "2380.TW", "2382.TW", "2387.TW", "2395.TW", "2397.TW", "2399.TW", "2405.TW", "2417.TW", "2424.TW", "2425.TW", "2442.TW", "2465.TW", "2495.TW", "3002.TW"
#                     , "3005.TW", "3013.TW", "3017.TW", "3022.TW", "3046.TW", "3057.TW", "3060.TW", "3231.TW", "3416.TW", "3494.TW", "3515.TW", "3652.TW", "3701.TW", "3706.TW", "3712.TW"
#                     , "4916.TW", "4938.TW", "5117.TW", "5128.TW", "5215.TW", "5235.TW", "5258.TW", "6166.TW", "6206.TW", "6230.TW", "6277.TW", "6414.TW", "6579.TW"
#                         , "6591.TW", "6669.TW", "8114.TW", "8163.TW", "8210.TW", "9912.TW"]


# stcok_name=["聯強","精技","燦坤","華立","增你強","威健","文曄","益登","蔚華科","全科"
#             ,"弘憶股","安馳","大聯大","崇越","豐藝","全國電","展碁國際","長華","陞泰","至上","文曄甲特","大聯大甲特","至上甲特"]

# stock_thecode=["2347.TW", "2414.TW", "2430.TW", "3010.TW", "3028.TW", "3033.TW", "3036.TW"
#                , "3048.TW", "3055.TW", "3209.TW", "3312.TW", "3528.TW", "3702.TW", "5434.TW"
#                , "6189.TW", "6281.TW", "6776.TW", "8070.TW", "8072.TW", "8112.TW", "3036A.TW", "3702A.TW", "8112A.TW"]

stcok_name=["味全","味王","大成","大飲","卜蜂","統一","愛之味","泰山","福壽"
            ,"台榮","福懋油","佳格","聯華","聯華食","大統益","天仁","黑松","興泰","宏亞","鮮活果汁-KY","南僑","臺鹽"]

stock_thecode=["1201.TW", "1203.TW", "1210.TW", "1213.TW", "1215.TW", "1216.TW"
            , "1217.TW", "1218.TW", "1219.TW", "1220.TW", "1225.TW", "1227.TW", "1229.TW", "1231.TW"
            , "1232.TW", "1233.TW", "1234.TW", "1235.TW", "1236.TW", "1256.TW", "1702.TW", "1737.TW"]

file=open('食品_長期.csv',mode='w',newline='')

for i in range(0,len(stock_thecode)):
    risk_vola(stcok_name[i],stock_thecode[i])
    
file.close()
