import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:smalec/providers/login.provider.dart';

class LoginScreen extends StatelessWidget {

  var _login = '';
  var _password = '';

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: Scaffold(
        body: Container(
          color: Colors.blueAccent,
          child: Center(
            child: Card(
              child: Container(
                child: Column(
                  children: <Widget>[
                    Container(
                      child: Text('Log in to your account', style: TextStyle(fontSize: 20),),
                      height: 70,),
                    TextField(
                      decoration: InputDecoration(hintText: 'Login'),
                      onChanged: (value) {
                        _login = value;
                      },),
                    TextField(
                      decoration: InputDecoration(hintText: 'Password'),
                      obscureText: true,
                      autocorrect: false,
                      onChanged: (value) {
                        _password = value;
                      },),
                    Container(
                      height: 90,
                      child: Align(
                        alignment: Alignment.bottomRight,
                        child: RaisedButton(
                          color: Colors.blueAccent,
                          child: Text("Sign In", style: TextStyle(color: Colors.white)),
                          onPressed: () {
                            context.read<LoginProvider>().signIn(_login, _password);
                          },),
                      ),
                    )
                ],),
                width: 300,
                height: 300,
                padding: EdgeInsets.all(20.0)),
            ),
          ),
        )
      ),
    );
  }

}