# LINE Notify
LINE Notifyのクライアント


## 使い方

### メッセージのみ
```csharp
var client = new LineNotifyClient("<your_token>");
await client.NotifyAsync("test");
```

### 画像付き
```csharp

var client = new LineNotifyClient("<your_token>");
var parameter = new NotifyParameter
{
    Message = "test",
    ImageUrl = "https://example/sample.jpg",
};

await client.NotifyAsync(parameter);
```