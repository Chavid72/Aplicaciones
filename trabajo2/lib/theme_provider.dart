import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';


class ThemeProvider with ChangeNotifier {
  ThemeMode _themeMode = ThemeMode.light;
  int _nivel = 4; // Nivel por defecto: 4 palabras

  ThemeMode get themeMode => _themeMode;
  int get nivel => _nivel;

  Future<void> toggleTheme(bool isDarkMode) async {
    _themeMode = isDarkMode ? ThemeMode.dark : ThemeMode.light;
    notifyListeners();

    final prefs = await SharedPreferences.getInstance();
    await prefs.setBool('isDarkMode', isDarkMode);
  }

  Future<void> setNivel(int nivel) async {
    _nivel = nivel;
    notifyListeners();

    final prefs = await SharedPreferences.getInstance();
    await prefs.setInt('nivel', nivel);
  }

  Future<void> loadTheme() async {
    final prefs = await SharedPreferences.getInstance();
    _themeMode = (prefs.getBool('isDarkMode') ?? false) ? ThemeMode.dark : ThemeMode.light;
    _nivel = prefs.getInt('nivel') ?? 4; // Cargar el nivel o usar 4 como predeterminado
    notifyListeners();
  }
}