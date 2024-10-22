package com.example.panelboton_clasetheme.ui.theme

class ReadQuestion {

    val questionsCocina = listOf(
        Question("¿Qué tipo de arroz se utiliza comúnmente para hacer paella? ",
            listOf("Arroz jazmín", "Arroz basmati", "Arroz salvaje", "Arroz bomba"),
            3),
        Question("¿Cuál es el ingrediente principal del gazpacho?",
            listOf("Zanahoria", "Pepino", "Cebolla", "Tomate"),
            3),
        Question("¿Qué es un roux?",
            listOf("Mezcla de mantequilla y harina para espesar", "Un tipo de pan", "Una salsa agridulce", "Un aderezo de ensaladas"),
            0),
        Question("¿Qué ingrediente no es necesario en una receta tradicional de mayonesa? ",
            listOf("Huevo", "Aceite", "Leche", "Mostaza"),
            2),
        Question("¿Qué tipo de queso se utiliza en una auténtica pizza margarita?",
            listOf("Queso cheddar", "Queso mozzarella", "Queso azul", "Queso brie"),
            1),
        Question("¿Qué función tiene el bicarbonato de sodio en la repostería?",
            listOf("Endulzar la masa", "Leudar o hacer que suba la masa", "Espesar la mezcla", "Dar color"),
            1),
        Question("¿Cuál es el método adecuado para cortar en juliana?",
            listOf("Cortar en cubos pequeños", "Cortar en rodajas gruesas", "Cortar en tiras finas", "Triturar los ingredientes"),
            2),
        Question("¿Qué significa “pochar” en términos de cocina?",
            listOf("Freír a alta temperatura", "Hornear a baja temperatura", "Asar sin aceite", "Cocinar a fuego lento con aceite hasta que quede blando"),
            3),
        Question("¿Cuál es el nombre de la salsa hecha con albahaca, ajo, piñones, queso y aceite de oliva?",
            listOf("Salsa Pesto", "Salsa Alfredo", "Salsa bechamel", "Salsa romesco"),
            0),
        Question("¿Qué se obtiene al batir claras de huevo hasta que estén firmes?",
            listOf("Salsa holandesa", "Mantequilla", "Merengue italiano", "Claras a punto de nieve"),
            3)
    )


    // videojuegos fisica cocina zoologia historia cine
    fun questionThem (index:Int):List<Question>{
        if (index == 0)
            return questionsCocina
        else if (index ==1)
            return questionsCocina
        else if (index ==2)
            return questionsCocina
        else if (index ==3)
            return questionsCocina
        else if (index ==4)
            return questionsCocina
        else
            return questionsCocina
    }
}