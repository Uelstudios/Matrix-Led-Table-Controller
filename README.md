# Matrix-Led-Table-Controller
This is a software I wrote to control a led table I build. The software is written in C# and runs on a raspberry pi that is connected to a fade candy. The software supports runnings Applications (basically little classes), connecting via a Webpage & Websockets, a game pad like a nes controller or controlling an application like a game over the mobile phone browser.

If you have plans to build something similar youself or you have any questions about the project and/or the software (because it is not very well documented) please do not hestitate to contact me at uelstudios@gmail.com. I will be happy to help you.

If you do not have a real led table but you would like to run the software you can do so. The software includes different "renderers". A renderer defines how the output (basically an image with 10 by 10 pixels) is displayed. You can output it to a console, send it over ethernet, send it to the serial output or just send it to a fade candy.

Also the software is very modular. It is super easy to add new modules like an application or a new renderer.

Note that animations are currently not supported. A work around is to just build an application that renders different frames on an animation. This is not very complicated.

# Parameters:
  - mute        Mute an output channel in the console. (default: none)
  - com         Com port for arduino communication for touch sensors. (default: COM5)
  - baud        Baud rate for arduino communication (default: 57600)
  - renderer    The renderer to use (default: fadecandy)
  - app         The app to run on startup (default: idle)
  
# Commands
Note that you will have to possibility to run commands. Type "help" to learn more.
  - help/?              Get help
  - exec [command]      Execute a command on the communication server.
  - log [msg]           Add to logs
  - whoami              Who is running this software
  - uptime              Time the software is running
  - tcp [msg]           Send a message over tcp
  - ws [msg]            Send a message over web sockets
  - echo [msg]          Print to console
  - clear               Clear console
  - beep                Play sound on host
  - status              Get status/state of software
  - shutdown            Shutdown the host
  - '.'                 Repeat last command
  - exit                Exit the software
  
Just try to run them and you will learn what they will do.

# Troubleshooting:
  - The application will crash on startup if there is no fade candy server listening on "127.0.0.1:7890". If you do not want to use a face candy you have to set the "-renderer" parameter.

# Look at the pictures to get an idea of the project!

![](https://lh3.googleusercontent.com/YMk41zdxPMoNc_0eAmqq11M4bT5TcztXFciPUs8GcCz3L9Cu2pANCkGEhfiz2JcchUaIoDSdMO052yZnVxJoAO_bk6-Y3G4PXSQWBzteVY1UpTdyNwSKlRZlsLmarX2vu6LzcCdr6cyH1V0sh6EXKZ3YkGPjXogNrpnLnMq9Mh9IkQPeeSxhPTyR2MMWcwG1pzFEr6lUSCrAFQjphD0mkhzSywEQnk4ANvz1NkotNtOxbqCTzx2Xzoynawj_SNCxjzOdSY0I68_BnYuGn9znR9dJ_ADvtuXWXFIP91d2KsqrPjP5XxM5sha0UwNh8TKgm_BekbxUWkIUwRXxaBkc_Upzg8umlXYO2sHZPkVcUw4-TNEL4ssaEr4hRsAeQIAwMYifoWoQn2wSpfL_KwRZ0DnStyUON9bbV-o3T_hgKGazhTQbXjcBKC2hCkYdM7R_5p-xaOJ-UNzacCL4DTCUo5FtRoxjwLMYfTIlkWByvDfh4_X5xNMhaen4K8_UeCG2bkEd9iQn_6p-QxwiEXMRqqIX8v662l-pDKsKG5nAkWlSf1-t5l-UOaQeeTy5FZcnRKKFGwHqWURKaqmWS4603S7bMeety2dS2yWMXE8TRnmaaDoftrFKK0hWePCIEYF7bZJDx9toh3mlWlSUgiGfaTx8kqGa7rA=w1741-h980-no)

![](https://lh3.googleusercontent.com/l1Za-ZwNRG5_cmsKoOKgJCtaWzJN2oJoDzqbUZPe7VAayAd_OD8MPnYODmeRTJ02HO1wpHhhDG0Z6yWffxHSOVkIXkJz3WP-DIsaxT1d_o-FkBgiiuAn7lF3G3LsOgdDrT3NgGBS6sxSr-FPWJ4vmJ0sWbohL_eJIYlcaPQrGETRIRg43J6NDnCc4_L7gER2ZqtysfX7ZyFO1S1HYsoiHpewruuZRBtk5M5n-7gtO0wP_53lpApkvHy6MzNDf4jgAgAgZdMEJTwybI1prZGjSLkzlueIq_cmM2-ik79Sx7WdwGWYc2ZRXKxNNH-UzOBaqtiMVDmBCbJrgHCk_NmT2T8dPcTYy_CWt8oHb893CWRaoNndGbhmLMzU7UaYZm_N73wc9TP5W-mMfRbcxKrtmCaUJ11ixcxZG0comUgvvpcg31GRw7UYi0RVszylY2Qof3GPlRByFvxzNt4C8GNBFR0k5otxx2Bx0EdySI3V-cf2r6T4jj73v7heK7vOr1G5zytfIhUoYuIeci9kMrvsmqnVA6Ig8C0WEdE-d7cIX_pplzph00_nEaTX0TZ9BqCkFkNjXLNymt_GYKLEkiII8BsHOgNbEmhjx9K-I7DUaDLx3_6Sn9BTPMTU3u6VUJYq4RyrYWePGc7uM4y7L837UKtqsazWB2Y=w1440-h810-no)

![](https://lh3.googleusercontent.com/v-3TWihP1PP2oRxCR-KfYtdh1D0A6IhR5v18SwYF7q8sl13U7lZrtJ1BC8sR9wROUpDrSvOuuZjJYDZuvnaEn2lUSHdmxISedGHuipZ87aVRVD1RV5BIDm-9yA0pJDKnzai3tf35QzHLAHQdZ_4Z3DyMX5wWu2FR0WxhiWk1OQY4_g9Gghlo9GEp8gx6OMA9PEloaZWJVE1MzHovhGLLHNn2G04r2gME1LpKqW1izvNIp0zvkK_Tl27ElDIbRqCva_8dqJ8JRg22vw2NJVEwijGCWcDJiML-BuwLFuYqQlM9of2uwNZHfcVCCw1vXH1415bX8ZZyq5mgvRVzxzjj8sR_50ho0ktH3j3zw-SrksEQIE6aGenGiK7_3xx-wNzmlOHBiRbPmQDabVI2Q1iClIbcFyXysoxxjB9zZ6mRP4yeaQ1e6MYk-KimID_wJ-7aeGv6peZjs8JfRbur1hcbBDuFs6EPyTdlfK7PTcIFDhyWKmdTs-r053JUWjihINP1qVrYN6lSk64aq46AxGQM19b1lQrU0e0vUEOrXSdNzGRoR0ehcbanl01szqRcS7dALUGcpWwjPvQ3Z3HQDfA0IUfH5oC6626OAY2eHORWydrD8FuPLK3mic-UCt38le-B0f1Q1LeVTWx5b3IMGLdwn8Wo2OSOynI=w1741-h980-no)

![](https://lh3.googleusercontent.com/_W2pkmsDKsSJBEuB8K97XlfvKMpTM_herf9i2hLntjHHTec52j6VEGWlX0_6btJ4dLFUkwYVFvsYFR2VqNn4sBvW8v63FBam9HoX0dPPrikqKdOCGGHFX3SAbhD-NWqUP61b39FudCdAI6iIMepxWvf_Omw7zO_hp48TdebdBBt2I_MAPF--i8b1uSBikaM9O_rgddxJoU2bN-NTBKW_J08kkKYDX7dyshsrkvlrHRlbka8g76WSyPC4FyRlBxPhjOA0JK0HnI0GSuIzezZxjNHrY6BIgwTHwGShngjoS9GPxcNiVdnNP4CpQFHLYJ7YMCj80KO-QNAFf3bDTUAWr_3oIcOvUK4FdtW6fK6X9WEjUuK3P9YxA40gXIloShV5rB0LXQABYKwXXOwK0VYqUHr_D0TahnmTW3jwOBHHzbvSSAN_hbb4Xx3fpQqN9pwgapkSDcP4MRl0JF2graJ4tBJkBd7-kqKdBbzDdzcsoFoMMLeDJYayqxpD2_FL4tvEbj0RhFdqZkJJDlh0cyNio3v8eJMqpvPPCBSgsUaw1cy86l3WzbeqIH2q7dj9VximYWEh_UTsnv0XnEi3_gEzwgTbMox0RYDy2Hm9M9XZq8Zl7n51vIaU3DnzHzi0Td5Mo8Skwr0gxcx6TuUrQQcs_T6wpE4FZgg=w1741-h980-no)


# Thank you for looking at my project.
I would like to mention that you are not allowed to use this software commercially without my consent.

If you use this software for a personal project or you just want to say something I would like to hear from you!
