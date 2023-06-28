# サンプルコマンド (基本編)

## サーバ用のコマンド

基本コマンド
```
syslogdotnet server <オプション1> <オプション2> <オプション3> ...
```

UDPで待ち受け
```
syslogdotnet server /u "0.0.0.0:514"
```

TCPで待ち受け (暗号化無し)
```
syslogdotnet server /t "0.0.0.0:514"
```

TCPで待ち受け (暗号化有り, クライアント証明書無し)
```
syslogdotnet server /t "0.0.0.0:514" /l /r "cert.crt" /w "<PASSWORD>"
```

TCPで待ち受け (暗号化有り, クライアント証明書有り)
```
syslogdotnet server /t "0.0.0.0:514" /l /r "cert.pfx" /w "<PASSWORD>" /q /e "cl.cert.cnname" 
```

UDPとTCPで同時待ち受け
```
syslogdotnet server /u "0.0.0.0:514" /t "0.0.0.0:514"
```

## クライアント用のコマンド

UDPのサーバに向けてメッセージ送信
```
syslogdotnet client /s "udp://192.168.1.100:514" /f RFC3164 /c Local0 /v info /m "Syslog Sample Message."
```

TCP(暗号化無し)に向けてメッセージ送信
```
syslogdotnet client /s "tcp://192.168.1.100:514" /f RFC3164 /c Local0 /v info /m "Syslog Sample Message."
```

TCP(暗号化有り, クライアント証明書無し)に向けてメッセージ送信
```
syslogdotnet client /s "tcp://192.168.1.100:514" /f RFC3164 /c Local0 /v info /m "Syslog Sample Message." /l
```

TCP(暗号化有り, クライアント証明書有り)に向けてメッセージ送信
```
syslogdotnet client /s "tcp://192.168.1.100:514" /f RFC3164 /c Local0 /v info /m "Syslog Sample Message." /l /r "cert.pfx" /w "<PASSWORD>"
```
