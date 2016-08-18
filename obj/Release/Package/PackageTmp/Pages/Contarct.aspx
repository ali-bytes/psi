<%@ Page Title=""   AutoEventWireup="true" CodeBehind="Contarct.aspx.cs" Inherits="NewIspNL.Pages.Contarct" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>عقد عملاء</title>
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <style type="text/css">
                #home{ direction: rtl;
               border: 2px solid black;
            padding: 25px;
        }
        #outer {
          border: 4px solid black;
            padding: 2px;
        }
        #form1 {
            border: 1px solid black;
            padding: 4px;
        }
        .rightheader {
            background-color: black;/*#B2ABAB;*/
            color: #ffffff;
            text-decoration: underline;
            font-size: 18px;
            font-weight: bold;
            padding: 22px;
            width: 100%;
            text-align: center;
            border-radius: 6px;
        }
        #headertable {
            height: 100px;
            margin-bottom: 22px;
        }
        #imgCo {
            width: 55%;
            height: 27%;
        }
        .h4format {
            background-color: black;
            color: white;
            padding: 8px;
            border-radius: 3px;
        }
                .h5format {
            background-color: #A19E9E;
            color: white;
            padding: 8px;
            border-radius: 3px;
        }
        table td,p{
            font-size: 15px;
font-weight: bold;
        }
        .smallcell {
            width: 16%;
        }
        .smallfont {
            font-size: 12px;
        }
         /*.smallcell2 {
            width: 25%;
        }
       .smallcell {
            width: 16%;
            font-size: 15px;
        }
        .datacell {
            width: auto;
        }
        table {
            direction: rtl;
            width: 100%;
        }
        .centertext {
            text-align: center;
        }

        .leftlabel {
            float: left;
        }
        .centerlabel {
            margin: 0 46% 0 0;
        }
        .imglogo {
            width: 35%;
height: 25%;
        }*/
        
         
    </style>
</head>
<body>
    <form id="form1" runat="server" clientidmode="Static">
    <div id="outer">
        <div id="home">
            <table width="100%" id="headertable">
                <tr>
                    <td style="width: 50%">
                        <div class="rightheader">
                            عقد اشتراك فى خدمة الانترنت<br />
                            ADSL Service Application Form
                        </div>
                    </td>
                    <td style="width: 50%;" align="left">
                        <asp:Image runat="server" ID="imgCo" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
            <table width="90%" dir="rtl">
                <tr>
                    <td class="smallcell">
                        عقد رقم :
                    </td>
                    <td>
                    </td>
                    <td class="smallcell">
                        الموظف :
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblEmployee"></asp:Label>
                    </td>
                    <td class="smallcell">
                        التاريخ :
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblDate"></asp:Label>
                    </td>
                </tr>
            </table>
            <h4 class="h4format">
                بيانات الاشتراك ( خاصة بالخط المراد توصيل الخدمة عليه )</h4>
            <table width="100%">
                <tr>
                    <td class="smallcell">
                        رقم التليفون  :<!--المراد توصيل الخدمة عليه-->
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblServicePhone"></asp:Label>
                    </td>
                    <td class="smallcell">
                        مالك الخط :<!--اسم المالك الأساسى لخط التليفون-->
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOwner"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="smallcell">
                        عنوان الخط  :<!--المراد توصيل الخدمة عليه بالتفصيل-->
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblAddress"></asp:Label>
                    </td>
                    <td colspan="2" class="smallfont">
                        يجب أن يكون التليفون أرضى ولا يوجد عليه خدمة ADSL من مقدم خدمات آخر *
                    </td>
                </tr>
            </table>
            <h4 class="h4format">
                الغرض من الاشتراك
            </h4>
            <table width="70%">
                <tr>
                    <td>
                        استخدام شخصى &nbsp;
                        <input type="checkbox" id="checkPersonalUse" />
                    </td>
                    <td>
                        مقهى انترنت &nbsp;
                        <input type="checkbox" id="checkCafe" />
                    </td>
                    <td>
                        استخدام شركة &nbsp;
                        <input type="checkbox" id="checkCompany" />
                    </td>
                </tr>
            </table>
            <h4 class="h4format">
                بيانات المشترك ( خاصة بالعميل المستفيد من الخدمة )</h4>
            <table width="100%">
                <tr>
                    <td class="smallcell">
                        اسم العميل :
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblCustomerName"></asp:Label>
                    </td>
                    <td class="smallcell">
                        الرقم القومى:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblPersonalId"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="smallcell">
                        رقم المحمول :
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblMobile1"></asp:Label>
                    </td>
                    <td class="smallcell">
                        E-mail:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblEmail"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        العنوان ( للمراسلات والفواتير ) :
                    </td>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblAddress2"></asp:Label>
                    </td>
                </tr>
            </table>
            <h4 class="h4format">
                تفاصيل الخدمة</h4>
            <table width="100%">
                <tr>
                    <td class="smallcell">
                        باقة الخدمة :
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblServicePackage"></asp:Label>
                    </td>
                    <td class="smallcell">
                        مزود الخدمة :
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblServiceProvider"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="smallcell">
                        العرض:
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblOffer"></asp:Label>
                    </td>
                    <td class="smallcell">
                        نوع الراوتر:
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table style="width: 70%;">
                            <tr>
                                <td>
                                    تكلفة الرواتر :
                                </td>
                                <td>
                                </td>
                                <td>
                                    مجانى
                                    <input type="checkbox" id="checkFreeRouter" />
                                </td>
                                <td>
                                    تكلفة مدفوعة
                                    <input type="checkbox" id="checkPaid" />
                                </td>
                                <td>
                                    قيمة الرواتر المدفوعة :
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <%--                <tr>
            <td class="smallcell">تكلفة الخدمة داخل العرض :</td>
            <td><asp:Label runat="server" ID="lblCostInOffer"></asp:Label></td>
            <td class="smallcell">تكلفة الخدمة بعد انتهاء العرض :</td>
            <td><asp:Label runat="server" ID="lblCostOutOffer"></asp:Label></td>
        </tr>
                <tr>

            <td class="smallcell">باقة اى بى :</td>
            <td><asp:Label runat="server" ID="lblIP"></asp:Label></td>
        </tr>--%>
            </table>
            <h4 class="h4format">
                خدمات اضافية أخرى</h4>
            <table>
                <tr>
                    <td>
                        خدمة IP ثابت :
                    </td>
                    <td>
                        1
                        <input type="checkbox" id="checkIp1" />
                    </td>
                    <td>
                        5
                        <input type="checkbox" id="checkIp5" />
                    </td>
                    <td>
                        13
                        <input type="checkbox" id="checkIp13" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        طلب زيارة لتفعيل الخدمة بتكلفة 75 جنيه :
                    </td>
                    <td>
                        أرغب
                        <input type="checkbox" id="checkIneedit" />
                    </td>
                    <td>
                        لا أرغب
                        <input type="checkbox" id="checkIdontNeed" />
                    </td>
                </tr>
                <%--                <tr>
            <td class="smallcell">مصاريف تعاقد :</td>
            <td><asp:Label runat="server" ID="lblContractCost"></asp:Label></td>
            <td class="smallcell">ايجار راوتر :</td>
            <td><asp:Label runat="server" ID="Label4"></asp:Label></td>
        </tr>
                <tr>
            <td class="smallcell">نوع الدفع :</td>
            <td><asp:Label runat="server" ID="lblPaymentType"></asp:Label></td>
            <td class="smallcell">تكلفة تحصيل من المنزل :</td>
            <td><asp:Label runat="server" ID="Label6"></asp:Label></td>
        </tr>
        <tr>
                        <td class="smallcell">مصاريف تأمين :</td>
            <td><asp:Label runat="server" ID="Label1"></asp:Label></td>
        </tr>--%>
            </table>
            <h4 class="h4format">
                اختيارات الدفع</h4>
            <table width="70%">
                <tr>
                    <td>
                        نقدى فى الشركة
                        <input type="checkbox" id="checkInComapny" />
                    </td>
                    <td>
                        ارسال مندوب<input type="checkbox" id="Checkbox1" />
                    </td>
                    <td>
                        تحويل بنكى<input type="checkbox" id="Checkbox2" />
                    </td>
                </tr>
            </table>
            <h4 class="h4format" style="margin-bottom: 0">
                الشروط والأحكام</h4>
            <h5 class="h5format" style="margin-top: 3px;">
                شروط عامة</h5>
            <p>
                1- يتعهد العميل بعدم توزيع أو بيع أو مشاركة الخدمة مع الاخرين عن طريق شبكات سلكيو
                أو لاسلكية سواء كان ذلك مجانا أو بأجر وفى حال حدوث ذلك يكون من حق الشركة ايقاف الخدمة
                فورا وبدون سابق انذار وابلاغ السلطات لاتخاذ الاجراءات القانونية اللازمة وتعتبر اى
                مبالغ مالية مدفوعة مقدما من العميل من حق الشركة على سبيل التعويض . 2- يقر العميل
                ببلوغه سن 21 عاما ليتحمل للالتزامات والشروط الوارده بهذا العقد .<br />
                3- اذا كان سبق تشغيل الخدمة على الخط موضوع التعاقد من شركة أخرى يكون على العميل
                احضار أمر الغاء الخدمة من الشركة الاخرى لتسهيل عملية التعاقد .<br />
                4- تلتزم الشركة بالمحافظة على سرية المعلومات الخاصة بالعملاء الا فى حالة طلب الجهات
                القضائية او بموافقة العميل .<br />
            </p>
            <h5 class="h5format">
                قيمة الاشتراك</h5>
            <p>
                1- يلتزم العميل بسداد قيمة الاشتراك فى/قبل موعد الاستحقاق وفقا للأسعار المعلنة بالاضافة
                الى اى مبالغ مالية عن فترات سابقة ( ان وجدت ) ، وفى حالة عدم سداد العميل للمستحقات
                يحق للشركة قطع الخدمة او انهاء التعاقد ويلتزم العميل بدفع قيمة اعادة توصيل الخدمة
                التى تقدرها الشركة عند اعادة تشغيل الخدمة .<br />
                2- جميع مبالغ الاشتراك غير قابلة للاسترداد الا فى حالة وجود مشاكل فنية من طرف الشركة
                تحول دون توفير الخدمة للعميل .<br />
                3- يجب على العميل الاحتفاظ بايصال السداد ولا تعتبر الشركة مسئولة عن اى مبالغ مدفوعة
                بدون تقديم ما يثبت ذلك من جهة العميل .<br />
                4- تلتزم الشركة بعدم تعديل الاشتراك الشهرى الا بعد الحصول على موافقة كتابية مسبقة
                من الجهاز القومى لتنظيم الاتصالات .<br />
            </p>
            <h5 class="h5format">
                ايقاف وانهاء الخدمة</h5>
            <p>
                1- يمكن للعميل ايقاف الخدمة مؤقتا مقابل مصاريف ايقاف شهرية طبقا للاسعار المعلنة
                وتسدد مصاريف الايقاف عن كل شهر توقف وتدفع مقدما .<br />
                2- للشركة الحق فى اتخاذ كافة الاجراءات اللازمة تجاه العميل اذا ثبت اساءة استخدام
                من جهة العميل بما يتنافى مع قانون الاتصالات او تعليمات الجهاز القومى لتنظيم الاتصالات
                او سياسة الاستخدام العادل للشركة ويعود تحديد اساءة الاستخدام من عدمه للشركة دون
                تدخل العميل .<br />
                3- تلتزم الشركة المزودة للخدمة بالغاء الخدمة للمستخدم بناء على طلبه فى مدة اقصاها
                15 يوم عمل من تاريخ تقديم الطلب بعد تصفية الاتزامات المالية مع الشركة واعادة الاجهزة
                المؤجرة فى الحالة التى كانت عليها عند الاستلام .<br />
                4- من حق الشركة ايقاف الخدمة مؤقتا او نهائيا اذا ما تأخر العميل عن سداد مقابل الخدمة
                او اى مصاريف اخرى مستحقة على العميل .<br />
            </p>
            <h5 class="h5format">
                مدة العقد</h5>
            <p>
                1- مدة العقد هى سنة من تاريخ اول تشغيل للخدمة وتجدد تلقائيا لمدة او مدد مماثلة ما
                لم يخطر احد الطرفين الطرف الاخر بعدم رغبته فى التجديد بمدة شهر قبل انتهاء مدته ويتم
                التجديد بنفس الشروط او بشروط اخرى .
            </p>
            <h5 class="h5format">
                المسئولية</h5>
            <p>
                1- الشركة غير مسئولة عن اعطال الخدمة التى تنتج فى خط التليفون او توصيلات التليفون
                الداخلية فى مكان العميل او جهاز الكمبيوتر الخاص بالعميل او اعمال شبكة الكمبيوتر
                الداخلية ان وجدت .<br />
                2- الشركة غير مسئولة عن اى اضرار مادية او معنوية قد تترتب على العميل نتيجة لسوء
                استخدام الخدمة .<br />
                3- تلتزم الشركة برد المبلغ المدفوع عند التعاقد فى حال طلب العميل عند عدم قدرة الشركة
                على تركيب الخط فى مدة اقصاها 3 اسابيع فى حال وجود اسباب ترجع الى الشركة ودة 6 اسابيع
                فى حالة ان يكون السبب خارج ارادة الشركة وتلتزم الشركة برد المبلغ المدفوع بعد خصم
                المصاريف الادارية المقررة للعميل فى حالة طلبه شرط احضار ايصال الدفع .<br />
                4- تلتزم الشركة ببذل اقصى جهد لتوفير الخدمة ولكنها لا تضمن ان تكون الخدمة خالية
                تماما من اى اعطال لاى اسباب قهرية خارجة عن ارادة الشركة .<br />
                5- للشركة الحق فى اجراء اى تعديلات او تغيرات على الخدمة المقدمة للعميل دون الحصول
                على موافقته اولا .<br />
                6- الشركة غير مسئولة عن محتوى مواقع الانترنت والملفات والانشطة التى يقوم بها العميل
                وكذلك جرائم الانترنت ( ويتم تفسيرها وفقا لاحكام القانون المصرى )عن طريق الخدمة وتقع
                المسئولية وتبعيات ذلك وحدها على عاتق العميل .<br />
            </p>
            <h4 class="h4format">
                اقرار</h4>
            <p>
                اقر باننى قرات و وافقت على جميع الشروط المكتوبة اعلاه وان جميع المعلومات السابق ذكرها
                صحيحه وأتحمل مسئوليتها
                <br/>
                <table width="100%">
                    <tr>
                        <td class="smallcell">اسم العميل :</td>
                        <td></td>
                        <td class="smallcell">رقم العميل :</td>
                        <td></td>
                        <td class="smallcell"></td>
                    </tr>
                                        <tr>
                        <td class="smallcell">الرقم القومى:</td>
                        <td></td>
                        <td class="smallcell">توقيع العميل:</td>
                        <td></td>
                        <td class="smallcell">توقيع الموظف :</td>
                    </tr>
                </table>
            </p>
            <hr />
            <br />
            <asp:Label runat="server" ID="lblfooterComapny"></asp:Label>
            <div runat="server" id="lblfooterContact">
            </div>
        </div>
    </div>
    <input type="button" id="btnPrint" value="طباعة العقد" style="height: 30px; margin: 2px; " />
    </form>
</body>
   
<script src="../Content/ace-assest/js/jquery-1.11.1.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#btnPrint").click(function () {
            window.print();
        });
    });
</script>

</html>
