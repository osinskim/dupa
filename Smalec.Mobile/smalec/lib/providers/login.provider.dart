import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:dio/dio.dart';

class LoginProvider with ChangeNotifier {
  final Dio _dio = new Dio();

  String _jwtToken = '';
  String get jwtToken => _jwtToken;

  String _refreshToken = '';
  String get refreshToken => _refreshToken;
  
  void signIn(String login, String password) {
    Future.microtask(() async  {
      Response response = await _dio.post(
        'http://192.168.0.129:80/Auth/SignIn',
        queryParameters: {'Password':'Admin123\$%^','UserName':'admin@admin.pl'});
      
      print(response.data.toString());
    });
    notifyListeners();
  }
}