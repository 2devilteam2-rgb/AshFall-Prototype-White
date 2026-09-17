# UI and Verbs
ashfall-memory-verb-remember = Вспомнить
ashfall-memory-verb-remember-tooltip = Освежить в памяти подробности ваших прошлых отношений с этим человеком.

# Mutual hints
ashfall-memory-hint-1 = Кто-то пристально на тебя посмотрел...
ashfall-memory-hint-2 = Ты чувствуешь на себе чей-то изучающий взгляд.
ashfall-memory-hint-3 = Показалось, или { GENDER($target) ->
    [female] эта девушка на мгновение замерла
   *[male] этот человек на мгновение замер
}, глядя на тебя?
ashfall-memory-hint-4 = Кажется, твоё лицо кому-то здесь знакомо.
ashfall-memory-hint-5 = Ты ловишь на себе странный взгляд, полный узнавания.

# Recognition fallback lines
ashfall-memory-recog-pos-work-1 = Стой... Так это же!..
ashfall-memory-recog-pos-work-2 = Погоди, я тебя знаю!
ashfall-memory-recog-pos-work-3 = Вот это встреча... знакомое лицо!

ashfall-memory-recog-pos-pers-1 = Ба, знакомые все лица!
ashfall-memory-recog-pos-pers-2 = Не может быть, это же ты!
ashfall-memory-recog-pos-pers-3 = Вот кого я точно не { GENDER($user) ->
    [female] ожидала
   *[male] ожидал
} здесь встретить... в хорошем смысле!

ashfall-memory-recog-neg-work-1 = Только не { GENDER($target) ->
    [female] эта...
   *[male] этот...
}
ashfall-memory-recog-neg-work-2 = Чёрт. Это же { GENDER($target) ->
    [female] та самая персона.
   *[male] тот самый кадр.
}
ashfall-memory-recog-neg-work-3 = Так-так, старые знакомые. Настроение испорчено.

ashfall-memory-recog-neg-pers-1 = О нет... только не { GENDER($target) ->
    [female] она.
   *[male] он.
}
ashfall-memory-recog-neg-pers-2 = Вот чёрт. Лицо кажется слишком знакомым...
ashfall-memory-recog-neg-pers-3 = Не может быть. Неужели это действительно { GENDER($target) ->
    [female] она?
   *[male] он?
}

ashfall-memory-recog-neutral-1 = Хм, знакомое лицо...
ashfall-memory-recog-neutral-2 = Где-то я уже это лицо { GENDER($user) ->
    [female] видела.
   *[male] видел.
}
ashfall-memory-recog-neutral-3 = Кажется, мы уже пересекались раньше.

ashfall-memory-recog-complicated-1 = Ну надо же... и как на это реагировать?
ashfall-memory-recog-complicated-2 = Знакомый силуэт... и ворох старых воспоминаний.
ashfall-memory-recog-complicated-3 = Слишком много всего всплывает в памяти...

# Recognition overrides
ashfall-memory-recognition-debt-debtor = Чёрт... только не { GENDER($target) ->
    [female] она. Надеюсь, она забыла
   *[male] он. Надеюсь, он забыл
} про долг...
ashfall-memory-recognition-debt-creditor = Стоп... Глазам не верю. Это же { GENDER($target) ->
    [female] моя должница!
   *[male] мой должник!
}

# Templates

# FireRescue
ashfall-memory-fire-rescue-a-text = Ты узнаёшь { GENDER($target) ->
    [female] её. Несколько лет назад вы работали в одной инженерной бригаде. Во время пожара она вытащила тебя из задымлённого техтоннеля, рискуя собственной шкурой.
   *[male] его. Несколько лет назад вы работали в одной инженерной бригаде. Во время пожара он вытащил тебя из задымлённого техтоннеля, рискуя собственной шкурой.
} Ты так и не { GENDER($user) ->
    [female] успела
   *[male] успел
} толком { GENDER($target) ->
    [female] её
   *[male] его
} отблагодарить.
ashfall-memory-fire-rescue-a-summary = { GENDER($target) ->
    [female] Вытащила
   *[male] Вытащил
} тебя из задымлённого техтоннеля во время пожара.
ashfall-memory-fire-rescue-b-text = Ты помнишь это лицо. Во время страшного пожара в техтоннелях на прошлой станции ты на себе { GENDER($user) ->
    [female] выволокла
   *[male] выволок
} { GENDER($target) ->
    [female] эту бедняжку
   *[male] этого бедолагу
} сквозь дым и пламя. Приятно видеть, что { GENDER($target) ->
    [female] она до сих пор жива
   *[male] он до сих пор жив
} и на ногах.
ashfall-memory-fire-rescue-b-summary = Ты { GENDER($user) ->
    [female] спасла
   *[male] спас
} { GENDER($target) ->
    [female] её
   *[male] его
} из огня в задымлённом техтоннеле.

# GoodCoworkers
ashfall-memory-good-coworkers-text = Вы раньше работали в одной смене. Между вами никогда не было лишних разговоров, но в работе вы понимали друг друга без слов. На { GENDER($target) ->
    [female] эту коллегу
   *[male] этого человека
} всегда можно было положиться.
ashfall-memory-good-coworkers-summary = { GENDER($target) ->
    [female] Надёжная коллега
   *[male] Надёжный коллега
} с вашей прошлой станции.

# DrinkingBuddies
ashfall-memory-drinking-buddies-text = Вы регулярно пересекались в баре после тяжёлых рабочих смен. Выпили не один литр дрянного дешёвого пойла, жаловались на начальство и травили байки. Приятные воспоминания о не самых простых временах.
ashfall-memory-drinking-buddies-summary = { GENDER($target) ->
    [female] Старая собутыльница
   *[male] Старый собутыльник
} после рабочих смен.

# HelpedWithMove
ashfall-memory-helped-move-a-text = Ты помнишь, как во время экстренной эвакуации или переезда в жилой сектор { GENDER($target) ->
    [female] эта женщина без лишних вопросов помогла
   *[male] этот человек без лишних вопросов помог
} тебе дотащить тяжеленные ящики с личными вещами. Мелочь, а врезалась в память.
ashfall-memory-helped-move-a-summary = { GENDER($target) ->
    [female] Помогла
   *[male] Помог
} тебе дотащить вещи во время переезда.
ashfall-memory-helped-move-b-text = Ты смутно припоминаешь, как однажды { GENDER($user) ->
    [female] помогла
   *[male] помог
} { GENDER($target) ->
    [female] ей
   *[male] ему
} с погрузкой и перетаскиванием барахла при переезде. Вы почти не общались после этого, но лицо запомнилось.
ashfall-memory-helped-move-b-summary = Ты когда-то { GENDER($user) ->
    [female] помогла
   *[male] помог
} { GENDER($target) ->
    [female] ей
   *[male] ему
} с вещами при переезде.

# AcademicRival
ashfall-memory-academic-rival-a-text = Лицо кажется слишком знакомым. Вы учились вместе. { GENDER($target) ->
    [female] Она всегда получала лучшие оценки, выпендривалась перед преподавателями и прекрасно знала о своём превосходстве. Ты до сих пор её
   *[male] Он всегда получал лучшие оценки, выпендривался перед преподавателями и прекрасно знал о своём превосходстве. Ты до сих пор его
} недолюбливаешь.
ashfall-memory-academic-rival-a-summary = { GENDER($target) ->
    [female] Бывшая сокурсница-выскочка, получавшая
   *[male] Бывший сокурсник-выскочка, получавший
} лучшие оценки.
ashfall-memory-academic-rival-b-text = Ты помнишь { GENDER($target) ->
    [female] её по учёбе. Она вечно плелась позади в рейтинге и бросала на тебя косые, завистливые взгляды всякий раз, когда тебя хвалили кураторы. Забавно видеть её
   *[male] его по учёбе. Он вечно плёлся позади в рейтинге и бросал на тебя косые, завистливые взгляды всякий раз, когда тебя хвалили кураторы. Забавно видеть его
} здесь.
ashfall-memory-academic-rival-b-summary = { GENDER($target) ->
    [female] Бывшая однокурсница, вечно завидовавшая
   *[male] Бывший однокурсник, вечно завидовавший
} твоим успехам.

# Debt250
ashfall-memory-debt-debtor-text = Чёрт. Ты { GENDER($target) ->
    [female] её
   *[male] его
} знаешь. Ты всё ещё { GENDER($user) ->
    [female] должна
   *[male] должен
} { GENDER($target) ->
    [female] этой женщине
   *[male] этому человеку
} 250 кредитов со старой попойки или карточной игры на прошлой станции. Главное — делать вид, что всё нормально, или надеяться, что { GENDER($target) ->
    [female] она не вспомнит.
   *[male] он не вспомнит.
}
ashfall-memory-debt-debtor-summary = Ты { GENDER($user) ->
    [female] должна
   *[male] должен
} { GENDER($target) ->
    [female] этой женщине
   *[male] этому человеку
} 250 кредитов.
ashfall-memory-debt-creditor-text = Вот так встреча! { GENDER($target) ->
    [female] Эта особа уже пару лет делает вид, что забыла про долг в 250 кредитов. Самое время напомнить ей
   *[male] Этот субъект уже пару лет делает вид, что забыл про долг в 250 кредитов. Самое время напомнить ему
} о старых счетах.
ashfall-memory-debt-creditor-summary = { GENDER($target) ->
    [female] Должна
   *[male] Должен
} тебе 250 кредитов со старых времён.

# StolenCredit
ashfall-memory-stolen-credit-victim-text = Ты прекрасно помнишь { GENDER($target) ->
    [female] эту женщину. Вы вместе работали над сложным отчётом, но когда пришло начальство, всю славу и премию она хладнокровно приписала
   *[male] этого человека. Вы вместе работали над сложным отчётом, но когда пришло начальство, всю славу и премию он хладнокровно приписал
} исключительно себе. Осадок остался на всю жизнь.
ashfall-memory-stolen-credit-victim-summary = { GENDER($target) ->
    [female] Присвоила себе твою работу и получила
   *[male] Присвоил себе твою работу и получил
} за неё премию.
ashfall-memory-stolen-credit-culprit-text = Ты смутно узнаёшь { GENDER($target) ->
    [female] эту коллегу. Вы когда-то сдавали общий проект, и руководство выписало премию именно тебе. Кажется, она тогда здорово надулась
   *[male] этого коллегу. Вы когда-то сдавали общий проект, и руководство выписало премию именно тебе. Кажется, он тогда здорово надулся
}, хотя ты просто умеешь подавать результаты лучше.
ashfall-memory-stolen-credit-culprit-summary = { GENDER($target) ->
    [female] Бывшая коллега, обидевшаяся
   *[male] Бывший коллега, обидевшийся
} из-за распределения премий.

# Whistleblower
ashfall-memory-whistleblower-reported-text = Из-за доноса { GENDER($target) ->
    [female] этой стукачки
   *[male] этого стукача
} тебя однажды лишили квартальной надбавки и подвергли унизительной проверке службы безопасности за сущее пустяковое нарушение регламента. Забыть такое сложно.
ashfall-memory-whistleblower-reported-summary = { GENDER($target) ->
    [female] Настучала
   *[male] Настучал
} на тебя руководству на прошлом месте работы.
ashfall-memory-whistleblower-reporter-text = Ты помнишь { GENDER($target) ->
    [female] её. На прошлой станции она грубейшим образом пренебрегала
   *[male] его. На прошлой станции он грубейшим образом пренебрегал
} техникой безопасности, и тебе пришлось доложить об этом в рапорте, чтобы не взлететь на воздух вместе с { GENDER($target) ->
    [female] ней.
   *[male] ним.
}
ashfall-memory-whistleblower-reporter-summary = Ты { GENDER($user) ->
    [female] подавала
   *[male] подавал
} на { GENDER($target) ->
    [female] неё
   *[male] него
} рапорт за нарушение техники безопасности.

# SameStation
ashfall-memory-same-station-text = Вы ходили по одним и тем же коридорам на прошлой станции и мелькали друг у друга перед глазами чуть ли не каждый день, хотя так ни разу толком и не заговорили. Знакомое до боли лицо из прошлой жизни.
ashfall-memory-same-station-summary = { GENDER($target) ->
    [female] Мелькала
   *[male] Мелькал
} перед глазами каждый день на прошлой станции.

# MutualAcquaintance
ashfall-memory-mutual-acquaintance-text = Вы никогда не были близки, но у вас был общий хороший знакомый со снабжения, который не раз упоминал вас обоих за кружкой пива. Мир тесен, раз вас занесло на одно и то же корыто.
ashfall-memory-mutual-acquaintance-summary = У вас есть общий хороший знакомый со старой станции.

# AsymmetricFriendship
ashfall-memory-asymmetric-friendship-a-text = Вы были близкими друзьями. По крайней мере, ты всегда так { GENDER($user) ->
    [female] считала
   *[male] считал
}: вы делились новостями, вместе проводили перерывы и поддерживали друг друга. Странно встретить { GENDER($target) ->
    [female] её
   *[male] его
} здесь.
ashfall-memory-asymmetric-friendship-a-summary = Ты { GENDER($user) ->
    [female] считала
   *[male] считал
} вас близкими друзьями на прошлой станции.
ashfall-memory-asymmetric-friendship-b-text = { GENDER($target) ->
    [female] Она всегда считала
   *[male] Он всегда считал
} вас близкими друзьями. Ты никогда не { GENDER($user) ->
    [female] понимала
   *[male] понимал
} почему — просто пара случайных разговоров в курилке, а { GENDER($target) ->
    [female] она уже привязалась
   *[male] он уже привязался
} как банный лист.
ashfall-memory-asymmetric-friendship-b-summary = { GENDER($target) ->
    [female] Считала
   *[male] Считал
} вас лучшими друзьями, хотя ты так не { GENDER($user) ->
    [female] думала.
   *[male] думал.
}

# FormerPartner
ashfall-memory-former-partner-a-text = Когда-то между вами было нечто большее, чем просто служебные отношения. Всё закончилось некрасиво и натянуто, оставив после себя глухую неловкость и нежелание ворошить прошлое.
ashfall-memory-former-partner-a-summary = Были сложные романтические отношения, закончившиеся разрывом.
ashfall-memory-former-partner-b-text = Лицо из прошлого. Вы когда-то пытались сблизиться, но это была явная ошибка. Сейчас даже странно вспоминать, что вас вообще могло связывать.
ashfall-memory-former-partner-b-summary = Былая любовная интрижка, о которой лучше не вспоминать.

# ── Pre-Mothballing Station Life & Cryo-sleep ─────────────────

# OldShiftHandover
ashfall-memory-old-shift-handover-text = В золотые годы станции вы не раз сдавали друг другу смену на посту. Чистые экраны, аккуратно заполненный бумажный журнал и горячий чай в кружке. Ни одного нарекания от руководства за всё время.
ashfall-memory-old-shift-handover-summary = Сдавали друг другу смену без единого нарекания.

# CafeteriaLunchTable
ashfall-memory-cafeteria-lunch-table-text = Вы часто делили столик в столовой жилого сектора, когда станция ещё жила полной жизнью, а в меню было настоящее мясо и свежие овощи. Спорили о спорте, жаловались на начальство и строили планы на отпуск.
ashfall-memory-cafeteria-lunch-table-summary = Обедали за одним столиком в столовой в лучшие годы станции.

# MothballOrderRumors
ashfall-memory-mothball-order-rumors-text = Вы стояли рядом в шлюзовом тамбуре и курили, когда по станции только поползли первые тревожные слухи о сокращении бюджета и грядущей консервации отсеков. Тогда в это ещё мало кто верил.
ashfall-memory-mothball-order-rumors-summary = Вместе обсуждали первые слухи о консервации станции.

# CryoQueueHandshake
ashfall-memory-cryo-queue-handshake-text = В день окончательной консервации сектора вы стояли плечом к плечу в длинной очереди в криоотсек. Перед тем как разойтись по капсулам, обменялись крепким рукопожатием и пожелали друг другу проснуться в лучшие времена.
ashfall-memory-cryo-queue-handshake-summary = Пожали друг другу руки в очереди на погружение в криосон.

# CryoPodPrepNeighbor
ashfall-memory-cryo-prep-neighbor-a-text = Ваши криокапсулы стояли по соседству. Перед погружением в холодный раствор тебя охватила паника — страх так и не проснуться. { GENDER($target) ->
    [female] Эта женщина тогда спокойно заговорила с тобой и помогла
   *[male] Этот человек тогда спокойно заговорил с тобой и помог
} справиться с дрожью в руках.
ashfall-memory-cryo-prep-neighbor-a-summary = { GENDER($target) ->
    [female] Помогла
   *[male] Помог
} тебе успокоиться перед погружением в криокапсулу.
ashfall-memory-cryo-prep-neighbor-b-text = Ваши капсулы консервации стояли рядом. Ты помнишь, как { GENDER($target) ->
    [female] эта бедняжка побелела от ужаса перед закрытием люка криокамеры.
   *[male] этот бедолага побелел от ужаса перед закрытием люка криокамеры.
} Ты пару минут { GENDER($user) ->
    [female] успокаивала
   *[male] успокаивал
} { GENDER($target) ->
    [female] её
   *[male] его
}, чтобы вправить мозги и не дать сорвать консервацию.
ashfall-memory-cryo-prep-neighbor-b-summary = Ты { GENDER($user) ->
    [female] успокаивала
   *[male] успокаивал
} { GENDER($target) ->
    [female] её, когда она паниковала
   *[male] его, когда он паниковал
} перед криосном.

# OvertimeDispute
ashfall-memory-overtime-dispute-a-text = В старые добрые времена корпорация щедро платила двойной тариф за праздничные дежурства. { GENDER($target) ->
    [female] Эта особа выбила
   *[male] Этот человек выбил
} себе ту жирную смену прямо у тебя из-под носа через связи с диспетчером. Мелочь, а осадок остался.
ashfall-memory-overtime-dispute-a-summary = { GENDER($target) ->
    [female] Увела
   *[male] Увёл
} у тебя выгодную сверхурочную смену по двойному тарифу.
ashfall-memory-overtime-dispute-b-text = Ты помнишь { GENDER($target) ->
    [female] её недовольную физиономию, когда диспетчер отдал праздничную сверхурочную смену с двойной оплатой тебе, а не ей.
   *[male] его недовольную физиономию, когда диспетчер отдал праздничную сверхурочную смену с двойной оплатой тебе, а не ему.
} Ты честно { GENDER($user) ->
    [female] отработала
   *[male] отработал
} те часы, но { GENDER($target) ->
    [female] она дулась
   *[male] он дулся
} ещё пару месяцев.
ashfall-memory-overtime-dispute-b-summary = { GENDER($target) ->
    [female] Обижалась
   *[male] Обижался
} на тебя из-за сверхурочной смены в праздник.

# ── Pre-Mothballing Medical ───────────────────────────────────

# MedicalRoutineCheckup
ashfall-memory-medical-routine-checkup-doctor-text = Ты помнишь { GENDER($target) ->
    [female] эту сотрудницу
   *[male] этого сотрудника
} по плановым ежегодным медосмотрам. Обычная корпоративная рутина: проверка рефлексов, зрение, давление и стандартная отметка в карте о допуске к высотным работам.
ashfall-memory-medical-routine-checkup-doctor-summary = { GENDER($target) ->
    [female] Проходила
   *[male] Проходил
} у тебя плановый ежегодный медосмотр.
ashfall-memory-medical-routine-checkup-patient-text = Ты узнаёшь своего врача. Когда станция ещё работала по регламенту, ты каждый год послушно { GENDER($user) ->
    [female] являлась
   *[male] являлся
} к { GENDER($target) ->
    [female] ней
   *[male] нему
} в кабинет на обязательный медосмотр и { GENDER($user) ->
    [female] выслушивала
   *[male] выслушивал
} дежурные шутки про уровень радиации.
ashfall-memory-medical-routine-checkup-patient-summary = { GENDER($target) ->
    [female] Твоя участковая врач
   *[male] Твой участковый врач
} с обязательных ежегодных медосмотров.

# MedicalJointSurgery
ashfall-memory-medical-joint-surgery-text = В доконсервационные времена вы плечом к плечу провели сложнейшую многочасовую полостную операцию в центральном медблоке. Работали на чистом профессионализме, понимая друг друга по взгляду через маску. Пациент выжил.
ashfall-memory-medical-joint-surgery-summary = Вместе провели образцовую сложную операцию в медблоке.

# MedicalSickLeavePass
ashfall-memory-medical-sick-leave-doctor-text = Ты помнишь, как перед самой консервацией { GENDER($target) ->
    [female] эта работница пришла
   *[male] этот рабочий пришёл
} к тебе с серым от усталости лицом. Ты { GENDER($user) ->
    [female] пожалела
   *[male] пожалел
} { GENDER($target) ->
    [female] бедняжку
   *[male] бедолагу
} и втихую { GENDER($user) ->
    [female] выписала
   *[male] выписал
} фиктивное «переутомление» на трое суток, прикрыв от грозившего штрафа.
ashfall-memory-medical-sick-leave-doctor-summary = Ты { GENDER($user) ->
    [female] выписала
   *[male] выписал
} { GENDER($target) ->
    [female] ей
   *[male] ему
} фиктивный отгул, спасая от выгорания.
ashfall-memory-medical-sick-leave-worker-text = Когда от переработок на старой станции у тебя уже темнело в глазах, { GENDER($target) ->
    [female] эта врач пошла на риск и оформила
   *[male] этот врач пошёл на риск и оформил
} фиктивный бюллетень о переутомлении. Три дня постельного режима спасли тебя от нервного срыва.
ashfall-memory-medical-sick-leave-worker-summary = { GENDER($target) ->
    [female] Втихую оформила
   *[male] Втихую оформил
} тебе больничный, когда ты { GENDER($user) ->
    [female] валилась
   *[male] валился
} с ног.

# ── Pre-Mothballing Security ──────────────────────────────────

# SecurityQuietNight
ashfall-memory-security-quiet-night-text = В те времена, когда на станции ещё царил образцовый порядок, вы вдвоём коротали спокойные ночные дежурства на КПП сектора. Пили чёрный кофе, травили анекдоты по служебной частоте и следили за тишиной в жилом блоке.
ashfall-memory-security-quiet-night-summary = Вместе дежурили на КПП в спокойные ночные смены.

# SecurityNoiseComplaint
ashfall-memory-security-noise-officer-text = Ты помнишь, как { GENDER($target) ->
    [female] эта жилица закатила дикую попойку с громкой музыкой по случаю закрытия полугодового контракта. Пришлось лично прийти в каюту и пригрозить карцером, чтобы она вырубила
   *[male] этот жилец закатил дикую попойку с громкой музыкой по случаю закрытия полугодового контракта. Пришлось лично прийти в каюту и пригрозить карцером, чтобы он вырубил
} аппаратуру.
ashfall-memory-security-noise-officer-summary = Ты { GENDER($user) ->
    [female] приходила
   *[male] приходил
} усмирять шумную пьянку в { GENDER($target) ->
    [female] её
   *[male] его
} каюте.
ashfall-memory-security-noise-resident-text = Ты смутно помнишь, как прямо в разгар отличной вечеринки перед отпуском { GENDER($target) ->
    [female] эта сотрудница безопасности вломилась
   *[male] этот сотрудник безопасности вломился
} в твою каюту с дубинкой наперевес и { GENDER($target) ->
    [female] угрожала
   *[male] угрожал
} сгноить всю компанию в изоляторе за нарушение ночного покоя.
ashfall-memory-security-noise-resident-summary = { GENDER($target) ->
    [female] Приходила
   *[male] Приходил
} с угрозами разогнать твою вечеринку в каюте.

# LostKeycardFiling
ashfall-memory-lost-keycard-officer-text = Ты помнишь { GENDER($target) ->
    [female] эту разиню. В доконсервационные времена она умудрилась посеять личную ключ-карту в вентиляционном люке, и тебе пришлось полчаса мурыжить её
   *[male] этого разиню. В доконсервационные времена он умудрился посеять личную ключ-карту в вентиляционном люке, и тебе пришлось полчаса мурыжить его
} длинной объяснительной перед выдачей дубликата.
ashfall-memory-lost-keycard-officer-summary = Ты { GENDER($user) ->
    [female] заставила
   *[male] заставил
} { GENDER($target) ->
    [female] её
   *[male] его
} писать нудный рапорт за утерю ключ-карты.
ashfall-memory-lost-keycard-worker-text = Ты прекрасно помнишь { GENDER($target) ->
    [female] эту бюрократку
   *[male] этого бюрократа
} в погонах. Когда ты случайно { GENDER($user) ->
    [female] обронила
   *[male] обронил
} карту доступа за обшивку, { GENDER($target) ->
    [female] она заставила
   *[male] он заставил
} тебя исписать три страницы рапорта о «халатном отношении к имуществу корпорации», прежде чем выдать дубликат.
ashfall-memory-lost-keycard-worker-summary = { GENDER($target) ->
    [female] Заставила
   *[male] Заставил
} тебя писать объяснительную за утерю пропуска.

# ── Pre-Mothballing Cargo & Logistics ─────────────────────────

# CargoUnloadingFreighter
ashfall-memory-cargo-unloading-freighter-text = В пору расцвета станции в доки каждую неделю приходили тяжелые грузовозы. Вы часами управляли грузовыми кранами и таскали тяжелые клети с оборудованием в слаженной связке, залитые потом, под ровный вой гидравлики.
ashfall-memory-cargo-unloading-freighter-summary = Разгружали тяжелые грузовые суда в доках в пору расцвета.

# CargoSpecialOrder
ashfall-memory-cargo-special-order-handler-text = Ты помнишь { GENDER($target) ->
    [female] эту женщину. По её
   *[male] этого человека. По его
} слёзной просьбе ты { GENDER($user) ->
    [female] вписала
   *[male] вписал
} в закрытый манифест снабжения пару пачек настоящего земного кофе и пачку любимых сигар в обход досмотра, передав посылку прямо в руки.
ashfall-memory-cargo-special-order-handler-summary = Ты { GENDER($user) ->
    [female] достала
   *[male] достал
} для { GENDER($target) ->
    [female] неё
   *[male] него
} редкий заказ через закрытый грузовой манифест.
ashfall-memory-cargo-special-order-client-text = Ты никогда не забудешь, как { GENDER($target) ->
    [female] эта девушка со склада помогла
   *[male] этот парень со склада помог
} тебе достать с грузового челнока настоящую земную посылку с кофе, которую официально на станцию было не заказать ни за какие деньги.
ashfall-memory-cargo-special-order-client-summary = { GENDER($target) ->
    [female] Достала
   *[male] Достал
} для тебя редкую посылку с Большой земли в обход правил.

# CargoCustomsHold
ashfall-memory-cargo-customs-inspector-text = Ты помнишь, как при { GENDER($target) ->
    [female] её
   *[male] его
} прибытии на станцию застрял подозрительный контейнер с личными вещами. Ты добросовестно { GENDER($user) ->
    [female] заперла
   *[male] запер
} багаж в досмотровой зоне на три дня до выяснения обстоятельств, выслушав от { GENDER($target) ->
    [female] неё
   *[male] него
} кучу претензий.
ashfall-memory-cargo-customs-inspector-summary = Ты { GENDER($user) ->
    [female] задержала
   *[male] задержал
} личный багаж { GENDER($target) ->
    [female] этой женщины
   *[male] этого человека
} на досмотре при прибытии на станцию.
ashfall-memory-cargo-customs-passenger-text = Ты помнишь { GENDER($target) ->
    [female] эту дотошную работницу. При твоём переводе на станцию она целых трое суток мариновала
   *[male] этого дотошного грузчика. При твоём переводе на станцию он целых трое суток мариновал
} твой личный сундук в грузовом шлюзе под предлогом «неправильно оформленной пломбы», заставив тебя спать на голой койке.
ashfall-memory-cargo-customs-passenger-summary = { GENDER($target) ->
    [female] Продержала
   *[male] Продержал
} твой багаж на таможне трое суток при заселении.

# ── Pre-Mothballing Engineering & Science ─────────────────────

# EngineeringGeneratorCommissioning
ashfall-memory-engineering-generator-commissioning-text = Вы вместе запускали в строй модернизированный распределительный узел энергосети станции за несколько лет до консервации. Стояли у главного терминала, сверяли частоты и гордились тем, что показатели легли ровно в допуски.
ashfall-memory-engineering-generator-commissioning-summary = Вместе запускали модернизированный энергоузел станции.

# EngineeringToolboxBorrow
ashfall-memory-engineering-toolbox-owner-text = Перед самой консервацией инженерного крыла { GENDER($target) ->
    [female] эта умница взяла у тебя фирменный калиброванный мультитул «буквально на полчаса подтянуть клеммы». Назад инструмент так и не вернулся — станцию законсервировали, а она развела
   *[male] этот умник взял у тебя фирменный калиброванный мультитул «буквально на полчаса подтянуть клеммы». Назад инструмент так и не вернулся — станцию законсервировали, а он развёл
} руками.
ashfall-memory-engineering-toolbox-owner-summary = { GENDER($target) ->
    [female] Одолжила
   *[male] Одолжил
} твой лучший инструмент перед консервацией и не { GENDER($target) ->
    [female] вернула.
   *[male] вернул.
}
ashfall-memory-engineering-toolbox-borrower-text = Ты смутно помнишь неловкую историю с { GENDER($target) ->
    [female] её
   *[male] его
} мультитулом. Ты { GENDER($user) ->
    [female] одолжила
   *[male] одолжил
} инструмент перед самой эвакуацией сектора, а потом началась суматоха с консервацией, и вернуть вещь { GENDER($target) ->
    [female] хозяйке
   *[male] хозяину
} ты попросту не { GENDER($user) ->
    [female] успела.
   *[male] успел.
}
ashfall-memory-engineering-toolbox-borrower-summary = Ты случайно { GENDER($user) ->
    [female] заиграла
   *[male] заиграл
} { GENDER($target) ->
    [female] её
   *[male] его
} инструмент в суматохе перед консервацией.

# ScienceCalibrationAssistance
ashfall-memory-science-calibration-engineer-text = Ты помнишь, как { GENDER($target) ->
    [female] эта научная сотрудница билась
   *[male] этот научный сотрудник бился
} над наладкой полевого спектрометра. Ты { GENDER($user) ->
    [female] потратила
   *[male] потратил
} полдня, перебрав { GENDER($target) ->
    [female] ей
   *[male] ему
} силовую проводку, а взамен { GENDER($target) ->
    [female] она рассчитала
   *[male] он рассчитал
} для тебя идеальный тепловой баланс контура охлаждения.
ashfall-memory-science-calibration-engineer-summary = Ты { GENDER($user) ->
    [female] наладила
   *[male] наладил
} { GENDER($target) ->
    [female] ей
   *[male] ему
} проводку в лаборатории в обмен на сложные расчеты.
ashfall-memory-science-calibration-scientist-text = Ты помнишь { GENDER($target) ->
    [female] эту толковую инженерку. Когда научный спектрометр сбоил из-за просадок сети, она быстро перепаяла силовой кабель в обход общей шины, спасла твои исследования,
   *[male] этого толкового инженера. Когда научный спектрометр сбоил из-за просадок сети, он быстро перепаял силовой кабель в обход общей шины, спас твои исследования,
} а ты { GENDER($user) ->
    [female] помогла
   *[male] помог
} { GENDER($target) ->
    [female] ей
   *[male] ему
} с математическими расчетами.
ashfall-memory-science-calibration-scientist-summary = { GENDER($target) ->
    [female] Починила
   *[male] Починил
} твой спектрометр в лаборатории за расчеты теплосети.

# ScienceSymposiumDebate
ashfall-memory-science-symposium-debate-text = Вы оба помните межсекторный научный симпозиум, где до хрипоты спорили о природе излучения местных аномалий. Каждый стоял на своём перед комиссией, так и не уступив ни пяди, но взаимное уважение к упорству коллеги осталось.
ashfall-memory-science-symposium-debate-summary = Яростно спорили о природе аномалий на научном симпозиуме.

# ── Pre-Mothballing Transit ───────────────────────────────────

# TransportLoungeChess
ashfall-memory-transport-lounge-chess-text = Во время бесконечного двухнедельного перелёта на пассажирском челноке к этой станции вы регулярно встречались в кают-компании и играли в шахматы. Никаких лишних разговоров о прошлом — только стук фигурок под гул маршевых двигателей.
ashfall-memory-transport-lounge-chess-summary = Играли в шахматы в кают-компании челнока по пути на станцию.
