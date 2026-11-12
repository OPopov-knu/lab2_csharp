<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html" encoding="utf-8" indent="yes" />

  <xsl:template match="/StudentParliament">
    <html>
      <head>
        <title>Студентський парламент</title>
      </head>
      <body>
        <h1>Студентський парламент</h1>
        <table border="1" cellpadding="6" cellspacing="0">
          <tr>
            <th>П.І.П.</th>
            <th>Факультет</th>
            <th>Відділення</th>
            <th>Кафедра</th>
            <th>Спеціальність</th>
            <th>Заходи</th>
          </tr>
          <xsl:apply-templates select="Member" />
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="Member">
    <tr>
      <td><xsl:value-of select="@fullName" /></td>
      <td><xsl:value-of select="@faculty" /></td>
      <td><xsl:value-of select="@division" /></td>
      <td><xsl:value-of select="@department" /></td>
      <td><xsl:value-of select="@specialty" /></td>
      <td>
        <xsl:for-each select="Event">
          <div>
            <xsl:value-of select="concat(@type, ' (', @startDate, ' — ', @endDate, ')')" />
          </div>
        </xsl:for-each>
      </td>
    </tr>
  </xsl:template>
</xsl:stylesheet>
