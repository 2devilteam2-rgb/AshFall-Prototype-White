# SPDX-License-Identifier: AGPL-3.0-or-later

click-damage-modifier = Лёгкие атаки (левая кнопка) { $arg ->
        [1] [color=green]на {$abs}% сильнее.[/color]
        *[other] [color=red]на {$abs}% слабее.[/color]
    }

heavy-damage-modifier = Тяжёлые атаки (правая кнопка) { $arg ->
        [1] [color=green]на {$abs}% сильнее.[/color]
        *[other] [color=red]на {$abs}% слабее.[/color]
    }
