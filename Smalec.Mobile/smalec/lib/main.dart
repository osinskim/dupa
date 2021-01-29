import 'package:flutter/material.dart';
import 'package:smalec/providers/login.provider.dart';
import 'package:smalec/screens/login.screen.dart';
import 'package:provider/provider.dart';

void main() => runApp(
  MultiProvider(
    providers: [
      ChangeNotifierProvider(create: (_) => LoginProvider())
    ],
    child: App(),
  )
);

class App extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return LoginScreen();
  }
}
