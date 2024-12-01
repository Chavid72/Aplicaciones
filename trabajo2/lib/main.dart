import 'package:shared_preferences/shared_preferences.dart';
import 'package:flutter/material.dart';
import 'package:audioplayers/audioplayers.dart';
import 'dart:async';
import 'dart:math';
import 'sopaLetras.dart';


void main() => runApp(SopaDeLetrasApp());

class SopaDeLetrasApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Find All',
      theme: ThemeData(primarySwatch: Colors.blue),
      home: SopaDeLetras(),
    );
  }
}

