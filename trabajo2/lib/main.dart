import 'package:shared_preferences/shared_preferences.dart';
import 'package:flutter/material.dart';
import 'package:audioplayers/audioplayers.dart';
import 'dart:async';
import 'dart:math';
import 'package:provider/provider.dart'; // Asegúrate de agregar el paquete provider
import 'sopaLetras.dart';
import 'theme_provider.dart';

void main() => runApp(SopaDeLetrasApp());

class SopaDeLetrasApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ChangeNotifierProvider(
      create: (_) => ThemeProvider()..loadTheme(),
      child: Consumer<ThemeProvider>(
        builder: (context, themeProvider, child) {
          return MaterialApp(
            title: 'Find All',
            theme: ThemeData.light(),
            darkTheme: ThemeData.dark(),
            themeMode: themeProvider.themeMode, // Usar el modo dinámico
            home: SopaDeLetras(),
          );
        },
      ),
    );
  }
}